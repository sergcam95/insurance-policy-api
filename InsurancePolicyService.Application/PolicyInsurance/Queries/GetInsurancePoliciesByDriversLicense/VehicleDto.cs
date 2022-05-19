namespace InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePoliciesByDriversLicense;

public class VehicleDto
{
    public int VehicleID { get; set; }

    public int Year { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Name { get; set; }
}