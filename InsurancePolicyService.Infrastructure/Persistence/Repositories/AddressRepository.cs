using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InsurancePolicyService.Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public AddressRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public Task<Address?> GetAddressByFullAddressNameAsync(int userId, string 
    fullAddressName, CancellationToken cancellationToken = default)
    {
        return _applicationDbContext.Addresses
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.UserID == userId && 
                                  e.FullAddress.ToLower().Equals(fullAddressName.ToLower()), cancellationToken);
    }
    
    public async Task<int> CreateAddressAsync(CreateAddress createAddress,
        CancellationToken cancellationToken = default)
    {
        var newAddress = (await _applicationDbContext.Addresses
            .AddAsync(new Address
            {
                FullAddress = createAddress.FullAddress,
                UserID = createAddress.UserID
            }, cancellationToken).ConfigureAwait(false)).Entity;

        await _applicationDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return newAddress.AddressID;
    }
}