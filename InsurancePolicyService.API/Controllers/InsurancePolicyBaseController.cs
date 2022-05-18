using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InsurancePolicyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsurancePolicyBaseController : ControllerBase
{
    private ISender _mediator = null!;
    
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}