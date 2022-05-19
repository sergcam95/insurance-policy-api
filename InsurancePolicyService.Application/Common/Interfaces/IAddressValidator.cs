namespace InsurancePolicyService.Application.Common.Interfaces;

public interface IAddressValidator
{
    Task<(bool IsValid, string? ErrorMessage)> ValidateAddressAsync(string address, 
    CancellationToken cancellationToken = default);
}