using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public Task<User?> GetUserByIDAsync(int userId, CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserID == userId, cancellationToken);
    }
    
    public Task<User?> GetUserByDriversLicenseNumberAsync(string driversLicenseNumber, 
        CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.DriversLicenseNumber == driversLicenseNumber,
                cancellationToken);
    }

    public async Task<int> CreateUserAsync(CreateUser createUser, 
        CancellationToken cancellationToken = default)
    {
        var newUser = (await _applicationDbContext.Users
            .AddAsync(new User
            {
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                DriversLicenseNumber = createUser.DriversLicenseNumber
            }, cancellationToken).ConfigureAwait(false)).Entity;

        await _applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return newUser.UserID;
    }
}