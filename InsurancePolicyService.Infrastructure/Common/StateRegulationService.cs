using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Models;

namespace InsurancePolicyService.Infrastructure.Common;

public class StateRegulationService : IStateRegulationService
{
    public (bool IsValid, string? ErrorMessage) ValidateInsurancePolicy(InsurancePolicyStateRegulation policy,
        CancellationToken cancellationToken = default)
    {
        // TODO: implement method
        return (true, null);
    }
}