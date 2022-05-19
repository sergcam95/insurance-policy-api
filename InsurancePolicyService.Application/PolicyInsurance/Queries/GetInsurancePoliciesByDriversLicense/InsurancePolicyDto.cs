
namespace InsurancePolicyService.Application.PolicyInsurance.Queries.GetInsurancePoliciesByDriversLicense;

public class InsurancePolicyDto
{
    public int InsurancePolicyID { get; set; }
    
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Premium { get; set; }
    
    public VehicleDto Vehicle { get; set; }
}