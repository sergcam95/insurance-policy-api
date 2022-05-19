using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Application.Common.Interfaces.Repositories;

public interface IInsurancePolicyRepository
{
    Task<int> CreateInsurancePolicyAsync(CreateInsurancePolicy createInsurancePolicy, 
        CancellationToken cancellationToken = default);

    Task<IEnumerable<InsurancePolicy>> GetInsurancePoliciesByDriversLicenseAsync(
        string driversLicense, bool? ascVehicleYear = null, bool includeExpiredPolicies = false,
        CancellationToken cancellationToken = default);

    Task<InsurancePolicy> GetInsurancePolicyByIdAndDriversLicense(
        int insurancePolicyId, string driversLicense,
        CancellationToken cancellationToken = default);
}