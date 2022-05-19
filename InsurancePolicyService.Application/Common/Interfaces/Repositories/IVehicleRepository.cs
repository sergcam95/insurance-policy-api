using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Application.Common.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetVehicleByNameAsync(int userId, string
        vehicleName, CancellationToken cancellationToken = default);
    Task<int> CreateVehicleAsync(CreateVehicle createVehicle, 
        CancellationToken cancellationToken = default);
}