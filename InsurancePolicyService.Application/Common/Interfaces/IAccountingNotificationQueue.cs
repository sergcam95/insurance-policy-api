using InsurancePolicyService.Application.Common.Models.Messages;

namespace InsurancePolicyService.Application.Common.Interfaces;

public interface IAccountingNotificationQueue
{
    Task QueueBackgroundWorkItemAsync(InsurancePolicyCreationMessage message);
    Task<InsurancePolicyCreationMessage> DequeueAsync(CancellationToken cancellationToken);
}