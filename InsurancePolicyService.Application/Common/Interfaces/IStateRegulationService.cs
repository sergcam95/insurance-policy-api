using InsurancePolicyService.Application.Common.Models;

namespace InsurancePolicyService.Application.Common.Interfaces;

public interface IStateRegulationService
{
    (bool IsValid, string? ErrorMessage) ValidateInsurancePolicy
    (InsurancePolicyStateRegulation policy, CancellationToken cancellationToken = default);
}