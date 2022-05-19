using InsurancePolicyService.Application.Common.Interfaces;

namespace InsurancePolicyService.Infrastructure.Common;

public class AddressValidator: IAddressValidator
{
    private readonly string[] _stateAbbreviations = new string[]
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA",
        "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA",
        "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY",
        "NC", "ND", "OH", "OK", "OR", "PA", "PR", "RI", "SC", "SD", "TN",   
        "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY"
    };
    
    public Task<(bool IsValid, string? ErrorMessage)> ValidateAddressAsync(string address, 
        CancellationToken cancellationToken = default)
    {
        var splitAddress = address.Split(",");

        // Format: "{addressLine}, {city}, {state} {ZipCode}"
        if (splitAddress.Length != 3)
            return Task.FromResult((false, "Address format is not correctly formatted"));

        // State and ZIP code part
        var stateAndZipCode = splitAddress[2].Trim().Split(" ");
        if(stateAndZipCode.Length != 2)
            return Task.FromResult((false, "State abbreviation and ZIP code are not correctly formatted"));
        
        // State
        if(!_stateAbbreviations.Contains(stateAndZipCode[0].ToUpper()))
            return Task.FromResult((false,$"State abbreviation {stateAndZipCode[0].ToUpper()} does not exist"));

        // ZIP code
        if(stateAndZipCode[1].Length != 5)
            return Task.FromResult((false,"ZIP code does not have 5 digits"));
        if (!int.TryParse(stateAndZipCode[1], out var zipCode))
            return Task.FromResult((false,"ZIP code is not a number"));
        if(zipCode < 1 || zipCode > 99950)
            return Task.FromResult((false,"ZIP code value is out of range"));
        
        return Task.FromResult((true, (string)null));
    }
}