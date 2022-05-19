using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Models;

namespace InsurancePolicyService.Infrastructure.Common;

public class StateRegulationService : IStateRegulationService
{
    private static readonly Random _random;
    
    static StateRegulationService()
    {
        _random = new Random();
    }

    public (bool IsValid, string? ErrorMessage) ValidateInsurancePolicy(InsurancePolicyStateRegulation policy,
        CancellationToken cancellationToken = default)
    {
        var randomNumber = _random.NextInt64(1, 4);
        return randomNumber switch
        {
            1 => (true, null),
            2 => (false, "State regulation #2"),
            _ => (false, "State regulation #3")
        };
    }
}