namespace InsurancePolicyService.Application.Common.Models;

public class InsurancePolicyStateRegulation
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DriversLicenseNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    
    public int VehicleYear { get; set; }
    public string VehicleModel { get; set; } = null!;
    public string VehicleManufacturer { get; set; } = null!;
    public string VehicleName { get; set; } = null!;
    
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Premium { get; set; }
}