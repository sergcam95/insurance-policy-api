namespace InsurancePolicyService.Domain;

public class Vehicle
{
    public int VehicleID { get; set; }

    public int Year { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Name { get; set; }

    public int UserID { get; set; }
}