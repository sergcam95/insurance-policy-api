using InsurancePolicyService.Application.PolicyInsurance.Commands.CreatePolicyInsurance;
using InsurancePolicyService.Application.PolicyInsurance.Queries.DTOs;
using InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePoliciesByDriversLicense;
using InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePolicyByIdAndDriversLicense;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyService.API.Controllers;

public class InsurancePolicyController : InsurancePolicyBaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatePolicyInsuranceDto>> CreateInsurancePolicyAsync
    (CreatePolicyInsuranceCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet]
    public async Task<IEnumerable<InsurancePolicyDto>> GetInsurancePoliciesByDriversLicenseAsync(
        [FromQuery] GetInsurancePoliciesByDriversLicenseQuery query)
    {
        return await Mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<InsurancePolicyDto>> GetInsurancePolicyByIdAndDriversLicenseAsync(
        int id,
        [FromQuery] string driversLicense)
    {
        var query = new GetInsurancePolicyByIdAndDriversLicenseQuery
        {
            InsurancePolicyId = id,
            DriversLicense = driversLicense
        };
        
        return await Mediator.Send(query);
    }
}