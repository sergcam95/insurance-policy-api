using System.Data.Common;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Application.Common.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<Address?> GetAddressByFullAddressNameAsync(int userId, string fullAddressName, 
    CancellationToken cancellationToken = default);
    Task<int> CreateAddressAsync(CreateAddress createAddress,
        CancellationToken cancellationToken = default);
}