namespace InsurancePolicyService.Application.Common.Models.Repositories;

public class CreateUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DriversLicenseNumber { get; set; } = null!;
}