using InsurancePolicyService.Application.PolicyInsurance.Commands.CreatePolicyInsurance;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyService.API.Controllers;

public class InsurancePolicyController : InsurancePolicyBaseController
{
    public async Task<ActionResult<CreatePolicyInsuranceDto>> CreateInsurancePolicyAsync
    (CreatePolicyInsuranceCommand command)
    {
        return await Mediator.Send(command);
    }
}