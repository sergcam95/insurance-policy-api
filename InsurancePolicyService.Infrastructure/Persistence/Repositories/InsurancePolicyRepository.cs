using InsurancePolicyService.Application.Common.Exceptions;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Infrastructure.Persistence.Repositories;

public class InsurancePolicyRepository : IInsurancePolicyRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public InsurancePolicyRepository(
        ApplicationDbContext applicationDbContext,
        IAddressRepository addressRepository,
        IUserRepository userRepository,
        IVehicleRepository vehicleRepository)
    {
        _applicationDbContext = applicationDbContext;
        _addressRepository = addressRepository;
        _userRepository = userRepository;
        _vehicleRepository = vehicleRepository;
    }
    
    public async Task<int> CreateInsurancePolicyAsync(CreateInsurancePolicy createInsurancePolicy, 
        CancellationToken cancellationToken = default)
    {
        if (createInsurancePolicy.Address == null)
            throw new RequestValidationException("Cannot create address because it is null");
        if (createInsurancePolicy.Vehicle == null)
            throw new RequestValidationException("Cannot create vehicle because it is null");

        await using var transaction = await _applicationDbContext.Database
            .BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var userId = createInsurancePolicy.UserID;
        if (userId.HasValue)
        {
            var user = await _userRepository.GetUserByIDAsync(userId.Value, cancellationToken)
                .ConfigureAwait(false);
            if (user == null)
                throw new RequestValidationException($"User with ID {userId} does not exist");
        }
        else
        {
            if (createInsurancePolicy.User == null)
                throw new RequestValidationException("Cannot create user because it is null");
            var user = await _userRepository.GetUserByDriversLicenseNumberAsync
                (createInsurancePolicy.User.DriversLicenseNumber, cancellationToken).ConfigureAwait(false);
            if (user != null)
                throw new RequestValidationException("User already exists with driver's license " +
                                                     $"{createInsurancePolicy.User.DriversLicenseNumber}");

            userId = await _userRepository.CreateUserAsync(createInsurancePolicy.User, 
            cancellationToken).ConfigureAwait(false);
        }

        var address = await _addressRepository.GetAddressByFullAddressNameAsync(userId.Value,
            createInsurancePolicy.Address.FullAddress, cancellationToken);
        var addressId = address?.AddressID;
        if (address == null)
        {
            createInsurancePolicy.Address.UserID = userId.Value;
            addressId = await _addressRepository.CreateAddressAsync(createInsurancePolicy
                .Address, cancellationToken: cancellationToken).ConfigureAwait
                (false);
        }

        var vehicle = await _vehicleRepository.GetVehicleByNameAsync(userId.Value,
            createInsurancePolicy.Vehicle.Name, cancellationToken).ConfigureAwait(false);
        var vehicleId = vehicle?.VehicleID;
        if (vehicle == null)
        {
            createInsurancePolicy.Vehicle.UserID = userId.Value;
            vehicleId = await _vehicleRepository.CreateVehicleAsync(createInsurancePolicy.Vehicle, 
            cancellationToken).ConfigureAwait(false);
            await _applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        
        var newInsurancePolicy = (await _applicationDbContext.InsurancePolicies.AddAsync(
            new InsurancePolicy
            {
                EffectiveDate = createInsurancePolicy.EffectiveDate,
                ExpirationDate = createInsurancePolicy.ExpirationDate,
                Premium = createInsurancePolicy.Premium,
                
                VehicleID = vehicleId.Value,
                UserID = userId.Value,
                AddressID = addressId.Value
            }, cancellationToken).ConfigureAwait(false)).Entity;

        await _applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        
        return newInsurancePolicy.InsurancePolicyID;
    }
}