namespace InsurancePolicyService.Application.Common.Models.Repositories;

public class CreateInsurancePolicy
{
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Premium { get; set; }

    public int? UserID { get; set; }
    public CreateUser? User { get; set; }
    public CreateAddress Address { get; set; } = null!;
    public CreateVehicle Vehicle { get; set; } = null!;
}