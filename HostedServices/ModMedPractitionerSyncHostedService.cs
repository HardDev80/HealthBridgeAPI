using HealthBridgeAPI.Services.Interfaces;

namespace HealthBridgeAPI.HostedServices
{
    public class ModMedPractitionerSyncHostedService: BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<ModMedPractitionerSyncHostedService> _logger;
        public ModMedPractitionerSyncHostedService(IServiceProvider provider, ILogger<ModMedPractitionerSyncHostedService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime utcNow = DateTime.UtcNow;

                DateTime estNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                    utcNow,
                    "Eastern Standard Time"
                );

                DateTime nextRun = estNow.Date.AddDays(1); // 12 AM EST next day
                TimeSpan delay = nextRun - estNow;

                _logger.LogInformation($"Next Practitioner sync in {delay.TotalMinutes} minutes.");

                await Task.Delay(delay, stoppingToken);

                using var scope = _provider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<IModMedPractitionerService>();

                try
                {
                    _logger.LogInformation("Running daily Practitioner sync...");
                    await svc.SyncPractitionersToDatabaseAsync();
                    _logger.LogInformation("Daily Practitioner sync completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during daily Practitioner sync");
                }
            }
        }
    }
}
