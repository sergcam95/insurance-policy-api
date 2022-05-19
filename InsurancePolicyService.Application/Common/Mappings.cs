using AutoMapper;
using InsurancePolicyService.Application.PolicyInsurance.Queries.DTOs;
using InsurancePolicyService.Domain;

namespace InsurancePolicyService.Application.Common;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<InsurancePolicy, InsurancePolicyDto>();
    }
}