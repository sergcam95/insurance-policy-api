using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Models.Messages;

namespace InsurancePolicyService.Infrastructure.Common;

public class AccountingService : IAccountingService
{
    private static readonly Random _random;
    static AccountingService()
    {
        _random = new Random();
    }
    
    public async Task NotifyAccountingServiceInsurancePolicyCreatedAsync
    (InsurancePolicyCreationMessage message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromSeconds(7), cancellationToken).ConfigureAwait(false);
        var value = _random.NextInt64(1, 3);
        if (value == 1)
            throw new Exception("Accounting service not available");
    }
}