using System.Threading.Channels;
using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Models.Messages;

namespace InsurancePolicyService.Infrastructure.Common;

public class AccountingNotificationQueue : IAccountingNotificationQueue
{
    private readonly Channel<InsurancePolicyCreationMessage> _queue;
    
    public AccountingNotificationQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<InsurancePolicyCreationMessage>(options);
    }
    
    public async Task QueueBackgroundWorkItemAsync(InsurancePolicyCreationMessage message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        await Task.Delay(TimeSpan.FromSeconds(20)).ConfigureAwait(false);

        await _queue.Writer.WriteAsync(message);
    }

    public Task<InsurancePolicyCreationMessage> DequeueAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAsync(cancellationToken).AsTask();
    }
}