namespace InsurancePolicyService.Domain;

public class InsurancePolicy
{
    public int InsurancePolicyID { get; set; }
    
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Premium { get; set; }

    public int UserID { get; set; }
    public User? User { get; set; }
    public int AddressID { get; set; }
    public int VehicleID { get; set; }
    public Vehicle? Vehicle { get; set; }
}