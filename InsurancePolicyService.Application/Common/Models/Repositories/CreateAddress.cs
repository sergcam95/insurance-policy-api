namespace InsurancePolicyService.Application.Common.Models.Repositories;

public class CreateAddress
{
    public string FullAddress { get; set; } = null!;

    public int UserID { get; set; }
}