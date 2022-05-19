using AutoMapper;
using FluentValidation;
using InsurancePolicyService.Application.Common.Exceptions;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.PolicyInsurance.Queries.DTOs;
using MediatR;

namespace InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePolicyByIdAndDriversLicense;

public class GetInsurancePolicyByIdAndDriversLicenseQuery : IRequest<InsurancePolicyDto>
{
    public int InsurancePolicyId { get; set; }
    public string DriversLicense { get; set; } = null!;
}

public class
    GetInsurancePolicyByIdAndDriversLicenseQueryValidators : AbstractValidator<
        GetInsurancePolicyByIdAndDriversLicenseQuery>
{
    public GetInsurancePolicyByIdAndDriversLicenseQueryValidators()
    {
        RuleFor(e => e.DriversLicense)
            .NotEmpty();
    }
}

public class GetInsurancePolicyByIdAndDriversLicenseQueryHandler :
    IRequestHandler<GetInsurancePolicyByIdAndDriversLicenseQuery, InsurancePolicyDto>
{
    private readonly IInsurancePolicyRepository _insurancePolicyRepository;
    private readonly IMapper _mapper;

    public GetInsurancePolicyByIdAndDriversLicenseQueryHandler(
        IInsurancePolicyRepository insurancePolicyRepository,
        IMapper mapper)
    {
        _insurancePolicyRepository = insurancePolicyRepository;
        _mapper = mapper;
    }
    
    public async Task<InsurancePolicyDto> Handle(GetInsurancePolicyByIdAndDriversLicenseQuery 
    request,
        CancellationToken cancellationToken)
    {
        var insurancePolicy = await _insurancePolicyRepository
            .GetInsurancePolicyByIdAndDriversLicense(
                request.InsurancePolicyId, request.DriversLicense, cancellationToken).ConfigureAwait
                (false);

        if (insurancePolicy == null)
            throw new NotFoundException($"Insurance Policy with ID " +
                                        $"{request.InsurancePolicyId} and driver's license " +
                                        $"{request.DriversLicense} not found");

        return _mapper.Map<InsurancePolicyDto>(insurancePolicy);
    }
}