namespace InsurancePolicyService.Domain;

public class User
{
    public int UserID { get; set; }
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DriversLicenseNumber { get; set; } = null!;
}