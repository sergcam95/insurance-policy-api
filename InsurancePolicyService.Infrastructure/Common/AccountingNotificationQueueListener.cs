using InsurancePolicyService.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace InsurancePolicyService.Infrastructure.Common;

public class AccountingNotificationQueueListener : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountingNotificationQueue _accountingNotificationQueue;
    private readonly ILogger<IAccountingService> _logger;

    public AccountingNotificationQueueListener(
        IServiceProvider serviceProvider,
        IAccountingNotificationQueue accountingNotificationQueue,
        ILogger<IAccountingService> logger)
    {
        _serviceProvider = serviceProvider;
        _accountingNotificationQueue = accountingNotificationQueue;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message =
                await _accountingNotificationQueue.DequeueAsync(stoppingToken)
                    .ConfigureAwait(false);
            
            try
            {
                await using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var accountingService = scope.ServiceProvider
                        .GetRequiredService<IAccountingService>();
                    
                    await Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(3, (retryAttempt) =>
                        {
                            _logger.LogError("Calling " +
                                             "NotifyAccountingServiceInsurancePolicyCreatedAsync\n" +
                                             $"Attempt: {retryAttempt}");
                            return TimeSpan.FromSeconds(7);
                        })
                        .ExecuteAsync(async () =>
                        {
                            await accountingService.NotifyAccountingServiceInsurancePolicyCreatedAsync(
                                message,
                                stoppingToken).ConfigureAwait(false);
                            
                            _logger.LogInformation("Accounting notification message was successfully processed!");
                        });
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    $"Error occurred when processing message for InsurancePolicy " +
                    $"{message.InsurancePolicyID}");
            }
        }
    }
}