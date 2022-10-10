using OnlineStore.Interface;

namespace OnlineStore.Services;

public class SendBackgroundService : BackgroundService
{
    private readonly ICurrentTime _currentTime;
    private readonly IServiceProvider _serviceProvider;

    public SendBackgroundService(IServiceProvider serviceProvider, ICurrentTime currentTime)
    {
        _serviceProvider = serviceProvider;
        _currentTime = currentTime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopeSendMessage = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            
        Console.WriteLine("Server started successfully at " + _currentTime.GetCurrentTimeLocal());

        while (!stoppingToken.IsCancellationRequested)
        {
            scopeSendMessage.Send(
                "PV011",
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