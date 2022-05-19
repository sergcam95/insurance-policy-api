using InsurancePolicyService.Application.Common.Models.Repositories;

namespace InsurancePolicyService.Application.Common.Interfaces.Repositories;

public interface IInsurancePolicyRepository
{
    Task<int> CreateInsurancePolicyAsync(CreateInsurancePolicy createInsurancePolicy, 
        CancellationToken cancellationToken = default);
}