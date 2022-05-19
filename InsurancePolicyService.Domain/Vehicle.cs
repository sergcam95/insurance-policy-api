namespace InsurancePolicyService.Domain;

public class Vehicle
{
    public int VehicleID { get; set; }

    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string Name { get; set; } = null!;

    public int UserID { get; set; }
}