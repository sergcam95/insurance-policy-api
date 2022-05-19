using System.Threading.Tasks;
using InsurancePolicyService.Infrastructure.Common;
using Xunit;

namespace InsurancePolicyService.Infrastructure.UnitTests.Common;

public class AddressValidatorUnitTests
{
    private readonly AddressValidator _addressValidator;
    
    public AddressValidatorUnitTests()
    {
        _addressValidator = new AddressValidator();
    }
    
    [Fact]
    public async Task
        ValidateAddressAsync_WhenAddressHasCorrectFormat_ShouldReturnTrueAndNullErrorMessage()
    {
        // arrange
        var address = "654 Test St, NiceCity, CA 12345";
        
        // act
        var result = await _addressValidator.ValidateAddressAsync(address)
            .ConfigureAwait(false);
        
        // assert
        Assert.True(result.IsValid);
        Assert.Null(result.ErrorMessage);
    }
    
    [Fact]
    public async Task
        ValidateAddressAsync_WhenAddressOnlyHasOneStringSeparatedByCommas_ShouldReturnFalseAndAnErrorMessage()
    {
        // arrange
        var address = "654 Test St";
        
        // act
        var result = await _addressValidator.ValidateAddressAsync(address)
            .ConfigureAwait(false);
        
        // assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }
    
    [Fact]
    public async Task
        ValidateAddressAsync_WhenStateDoesNotExist_ShouldReturnFalseAndAnErrorMessage()
    {
        // arrange
        var address = "654 Test St, NiceCity, FC 99933";
        
        // act
        var result = await _addressValidator.ValidateAddressAsync(address)
            .ConfigureAwait(false);
        
        // assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }
    
    [Fact]
    public async Task
        ValidateAddressAsync_WhenZipCodeIsNotANumber_ShouldReturnFalseAndAnErrorMessage()
    {
        // arrange
        var address = "654 Test St, NiceCity, CA 1#wer";
        
        // act
        var result = await _addressValidator.ValidateAddressAsync(address)
            .ConfigureAwait(false);
        
        // assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }
    
    [Fact]
    public async Task
        ValidateAddressAsync_WhenZipCodeDoesNotHave5Digits_ShouldReturnFalseAndAnErrorMessage()
    {
        // arrange
        var address = "654 Test St, NiceCity, CA 123456";
        
        // act
        var result = await _addressValidator.ValidateAddressAsync(address)
            .ConfigureAwait(false);
        
        // assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }
    
    [Fact]
    public async Task
        ValidateAddressAsync_WhenZipCodeDoesHas5DigitsButIsOutOfRange_ShouldReturnFalseAndAnErrorMessage()
    {
        // arrange
        var address = "654 Test St, NiceCity, CA 99951";
        
        // act
        var result = await _addressValidator.ValidateAddressAsync(address)
            .ConfigureAwait(false);
        
        // assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }
}