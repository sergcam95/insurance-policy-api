using AutoMapper;
using InsurancePolicyService.Application.Common.Models;
using InsurancePolicyService.Application.PolicyInsurance.Commands.CreatePolicyInsurance;
using InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePoliciesByDriversLicense;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Application.Common;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<InsurancePolicy, InsurancePolicyDto>();

        CreateMap<CreatePolicyInsuranceCommand, InsurancePolicyStateRegulation>();
    }
}