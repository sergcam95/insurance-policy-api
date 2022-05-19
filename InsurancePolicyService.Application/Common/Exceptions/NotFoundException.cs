namespace InsurancePolicyService.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string errorMessage): base(errorMessage) { }
}