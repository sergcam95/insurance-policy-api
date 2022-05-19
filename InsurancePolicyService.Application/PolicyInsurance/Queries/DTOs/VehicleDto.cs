namespace InsurancePolicyService.Application.PolicyInsurance.Queries.DTOs;

public class VehicleDto
{
    public int VehicleID { get; set; }

    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string Name { get; set; } = null!;
}