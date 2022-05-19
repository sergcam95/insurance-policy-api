namespace InsurancePolicyService.Domain;

public class Address
{
    public int AddressID { get; set; }

    public string FullAddress { get; set; } = null!;

    public int UserID { get; set; }
}