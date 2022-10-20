using OnlineStore.Interface;
using Polly;
using Polly.Retry;

namespace OnlineStore.Services;

public class SendBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IClock _currentTime;
    private readonly ILogger<SendBackgroundService> _logger;

    public SendBackgroundService(IServiceProvider serviceProvider, IClock currentTime, 
        IHostApplicationLifetime applicationLifetime ,ILogger<SendBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _currentTime = currentTime;
        _logger = logger;
        applicationLifetime.ApplicationStarted.Register(() =>
        {
            _logger.LogInformation("Server started successfully");
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var scopeSendMessage = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            
        Console.WriteLine("Server started successfully at " + _currentTime.GetCurrentTimeLocal());
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var to = "windows84@rambler.ru";
            const int attemptsLimit = 2;

            AsyncRetryPolicy? policy = Policy.Handle<Exception>()
                .RetryAsync(attemptsLimit, onRetry: (exception, retryCount) =>
                {
                    _logger.LogWarning(exception,
                        "Exception: {Service}, {Recipient}. Попытка №{Attempt}",
                        scopeSendMessage.GetType(), to, retryCount
                    );
                });

            PolicyResult? result = await policy.ExecuteAndCaptureAsync(
                _ => scopeSendMessage.SendAsync("PV011",
                    to,
                    "Server operation",
                    "Server is working properly!" +
                    "<br>Total memory: " +
                    GC.GetTotalMemory(false) + " bytes."), stoppingToken);
            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(result.FinalException,
                    "Exception: {Service}, {Recipient}",
                    scopeSendMessage.GetType(), to
                );
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }

        /*while (!stoppingToken.IsCancellationRequested)
        {
            var to = "windows84@rambler.ru";
            var sendingSucceeded = false;
            const int attemptsLimit = 2;
            for(int attemptsCount = 1; !sendingSucceeded && attemptsCount <= attemptsLimit; attemptsCount++)
            {
                try
                {
                    await scopeSendMessage.SendAsync(
                        "PV011",
                        to,
                        "Server operation",
                        "Server is working properly!" +
                        "<br>Total memory: " +
                        GC.GetTotalMemory(false) + " bytes."
                    );
                    sendingSucceeded = true;
                }
                catch (Exception e) when (attemptsCount < attemptsLimit)
                {
                    // Не последняя попытка
                    _logger.LogWarning(e,
                        "Exception: {Service}, {Recipient}. Попытка №{Attempt}",
                        scopeSendMessage.GetType(), to, attemptsCount
                    );
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        "Exception: {Service}, {Recipient}",
                        scopeSendMessage.GetType(), to
                    );
                }
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }*/
    }
}