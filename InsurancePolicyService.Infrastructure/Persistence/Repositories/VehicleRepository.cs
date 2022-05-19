using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyService.Infrastructure.Persistence.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public VehicleRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public Task<Vehicle?> GetVehicleByNameAsync(int userId, string 
        vehicleName, CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Vehicles
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.UserID == userId && 
                                       e.Name.ToLower().Equals(vehicleName.ToLower()), 
                                       cancellationToken);
    }
    
    public async Task<int> CreateVehicleAsync(CreateVehicle createVehicle, 
        CancellationToken cancellationToken = default)
    {
        var newVehicle = (await _applicationDbContext.Vehicles
            .AddAsync(new Vehicle
            {
                Name = createVehicle.Name,
                Model = createVehicle.Model,
                Manufacturer = createVehicle.Manufacturer,
                Year = createVehicle.Year,
                UserID = createVehicle.UserID
            }, cancellationToken).ConfigureAwait(false)).Entity;

        await _applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return newVehicle.VehicleID;
    }
}