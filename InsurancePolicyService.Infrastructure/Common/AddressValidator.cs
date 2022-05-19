using InsurancePolicyService.Application.Common.Interfaces;

namespace InsurancePolicyService.Infrastructure.Common;

public class AddressValidator: IAddressValidator
{
    public Task<(bool IsValid, string? ErrorMessage)> ValidateAddressAsync(string address, 
    CancellationToken cancellationToken = default)
    {
        // TODO: implement method
        return Task.FromResult<(bool, string)>((true, null));
    }
}