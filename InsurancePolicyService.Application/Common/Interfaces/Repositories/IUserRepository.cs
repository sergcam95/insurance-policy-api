using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIDAsync(int userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserByDriversLicenseNumberAsync(string driversLicenseNumber, 
        CancellationToken cancellationToken = default);

    Task<int> CreateUserAsync(CreateUser createUser,
        CancellationToken cancellationToken = default);
}