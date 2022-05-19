using InsurancePolicyService.Application.Common.Models.Messages;

namespace InsurancePolicyService.Application.Common.Interfaces;

public interface IAccountingService
{
    Task NotifyAccountingServiceInsurancePolicyCreatedAsync(InsurancePolicyCreationMessage message, 
        CancellationToken cancellationToken = default);
}