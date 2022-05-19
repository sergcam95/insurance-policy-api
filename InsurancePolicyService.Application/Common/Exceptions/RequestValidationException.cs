namespace InsurancePolicyService.Application.Common.Exceptions;

public class RequestValidationException : Exception
{
    public RequestValidationException(string errorMessage): base(errorMessage) { }
}