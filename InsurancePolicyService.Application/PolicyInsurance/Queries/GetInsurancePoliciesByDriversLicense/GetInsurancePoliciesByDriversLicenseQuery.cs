using AutoMapper;
using FluentValidation;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Domain;
using MediatR;

namespace InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePoliciesByDriversLicense;

public class GetInsurancePoliciesByDriversLicenseQuery : IRequest<IEnumerable<InsurancePolicyDto>>
{
    public string DriversLicense { get; set; }
    public bool? AscVehicleYear { get; set; }
    public bool IncludeExpiredPolicies { get; set; } = false;
}

public class
    GetInsurancePoliciesByDriversLicenseQueryValidator : AbstractValidator<
        GetInsurancePoliciesByDriversLicenseQuery>
{
    public GetInsurancePoliciesByDriversLicenseQueryValidator()
    {
        RuleFor(e => e.DriversLicense).NotEmpty();
    }
}

public class GetInsurancePoliciesByDriversLicenseQueryHandler :
    IRequestHandler<GetInsurancePoliciesByDriversLicenseQuery, IEnumerable<InsurancePolicyDto>>
{
    private readonly IInsurancePolicyRepository _insurancePolicyRepository;
    private readonly IMapper _mapper;

    public GetInsurancePoliciesByDriversLicenseQueryHandler(
        IInsurancePolicyRepository insurancePolicyRepository,
        IMapper mapper)
    {
        _insurancePolicyRepository = insurancePolicyRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<InsurancePolicyDto>> Handle
    (GetInsurancePoliciesByDriversLicenseQuery request, 
        CancellationToken cancellationToken)
    {
        var insurancePolicies = await _insurancePolicyRepository
        .GetInsurancePoliciesByDriversLicenseAsync(
            request.DriversLicense,
            request.AscVehicleYear, request.IncludeExpiredPolicies, cancellationToken)
            .ConfigureAwait(false);
        
        return insurancePolicies.Select(e => 
            _mapper.Map<InsurancePolicyDto>(e));
    }
}