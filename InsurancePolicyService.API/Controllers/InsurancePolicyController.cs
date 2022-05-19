using InsurancePolicyService.Application.PolicyInsurance.Commands.CreatePolicyInsurance;
using InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePoliciesByDriversLicense;
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
}