namespace Adoptrix.Initializer;

public class WorkerService(ILogger<WorkerService> logger, IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var executionCount = 0;
        while (!stoppingToken.IsCancellationRequested && executionCount < 5)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {Time}, execution count; {ExecutionCount}",
                    DateTimeOffset.Now, executionCount);
            }

            await Task.Delay(1000, stoppingToken);
            executionCount++;
        }

        logger.LogInformation("Work completed");
        hostApplicationLifetime.StopApplication();
    }
}
