using OnlineStore.Interface;

namespace OnlineStore.Services;

public class SendBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SendBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopeCurrentTime = scope.ServiceProvider.GetRequiredService<ICurrentTime>();
        var scopeSendMessage = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            
        Console.WriteLine("Server started successfully at " + scopeCurrentTime.GetCurrentTimeLocal());

        while (!stoppingToken.IsCancellationRequested)
        {
            scopeSendMessage.Send("PV011",
                "asp2022pd011@rodion-m.ru",
                "windows84@rambler.ru",
                "Server operation",
                "Server is working properly!" +
                "<br>Total memory: " + 
                GC.GetTotalMemory(false) + " bytes."
            );
            
            await Task.Delay(new TimeSpan(1, 0, 0));
        }
    }
}