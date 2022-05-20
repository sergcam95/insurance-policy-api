using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InsurancePolicyService.Application.Common;
using InsurancePolicyService.Application.Common.Exceptions;
using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Application.PolicyInsurance.Commands.CreatePolicyInsurance;
using Moq;
using Xunit;

namespace InsurancePolicyService.Application.UnitTests.PolicyInsurance.Commands.CreatePolicyInsurance;

public class CreatePolicyInsuranceCommandUnitTests
{
    private Mock<IAddressValidator> _mockAddressValidator;
    private Mock<IStateRegulationService> _mockstateRegulationService;
    private Mock<IInsurancePolicyRepository> _mockInsurancePolicyRepository;
    private Mock<IAccountingNotificationQueue> _mockAccountingNotificationQueue;

    private readonly CreatePolicyInsuranceCommandHandler _createPolicyInsuranceCommandHandler;
    
    public CreatePolicyInsuranceCommandUnitTests()
    {
        _mockAddressValidator = new Mock<IAddressValidator>();
        _mockstateRegulationService = new Mock<IStateRegulationService>();
        _mockInsurancePolicyRepository = new Mock<IInsurancePolicyRepository>();
        _mockAccountingNotificationQueue = new Mock<IAccountingNotificationQueue>();
        
        _mockAddressValidator
            .Setup(x => x.ValidateAddressAsync(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, null));
        
        _mockstateRegulationService
            .Setup(x => x.ValidateInsurancePolicy(
                It.IsAny<InsurancePolicyStateRegulation>(), It.IsAny<CancellationToken>()))
            .Returns((true, null));

        var autoMapperConfig = new MapperConfiguration(x => 
            x.AddProfile(new Mappings()));
        var mapper = autoMapperConfig.CreateMapper();
        
        _createPolicyInsuranceCommandHandler = new CreatePolicyInsuranceCommandHandler(
            _mockAddressValidator.Object,
            _mockstateRegulationService.Object,
            _mockInsurancePolicyRepository.Object,
            _mockAccountingNotificationQueue.Object, 
            mapper
        );
    }

    [Fact]
    public async Task ShouldThrowRequestValidationException_WhenVehicleYearGreaterThan1998()
    {
        // arrange
        var newInsurancePolicy = new CreatePolicyInsuranceCommand
        {
            VehicleYear = 2000
        };
        
        // act, assert
        await Assert.ThrowsAsync<RequestValidationException>(async () =>
            await _createPolicyInsuranceCommandHandler.Handle(newInsurancePolicy, CancellationToken.None));
    }
    
    [Fact]
    public async Task ShouldThrowRequestValidationException_WhenAddressIsInvalid()
    {
        // arrange
        var newInsurancePolicy = new CreatePolicyInsuranceCommand
        {
            VehicleYear = 1970,
            Address = "incorrect_address"
        };
        var expectedAddressValidatorErrorMessage = "Address not valid";

        _mockAddressValidator
            .Setup(x => x.ValidateAddressAsync(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, expectedAddressValidatorErrorMessage));
        
        // act, assert
        var thrownException = await Assert.ThrowsAsync<RequestValidationException>(async () =>
            await _createPolicyInsuranceCommandHandler.Handle(newInsurancePolicy, CancellationToken.None));
        
        Assert.Equal(thrownException.Message, expectedAddressValidatorErrorMessage);
    }
    
    [Fact]
    public async Task 
    ShouldThrowRequestValidationException_WhenEffectiveDateIsNotAtLeast30DaysAfterCreationDate()
    {
        // arrange
        var newInsurancePolicy = new CreatePolicyInsuranceCommand
        {
            VehicleYear = 1970,
            Address = "correct address",
            EffectiveDate = DateTime.UtcNow.AddDays(20)
        };

        // act, assert
        await Assert.ThrowsAsync<RequestValidationException>(async () =>
            await _createPolicyInsuranceCommandHandler.Handle(newInsurancePolicy, CancellationToken.None));
    }
    
    [Fact]
    public async Task 
        ShouldThrowRequestValidationException_WhenExpirationDateIsBeforeEffectiveDate()
    {
        // arrange
        var newInsurancePolicy = new CreatePolicyInsuranceCommand
        {
            VehicleYear = 1970,
            Address = "correct address",
            EffectiveDate = DateTime.UtcNow.AddDays(40),
            ExpirationDate = DateTime.UtcNow.AddDays(20)
        };

        // act, assert
        await Assert.ThrowsAsync<RequestValidationException>(async () =>
            await _createPolicyInsuranceCommandHandler.Handle(newInsurancePolicy, CancellationToken.None));
    }
    
    [Fact]
    public async Task ShouldThrowRequestValidationException_WhenStateRegulationReturnsAnError()
    {
        // arrange
        var newInsurancePolicy = new CreatePolicyInsuranceCommand
        {
            VehicleYear = 1970,
            Address = "correct, address",
            EffectiveDate = DateTime.UtcNow.AddDays(40),
            ExpirationDate = DateTime.UtcNow.AddDays(100)
        };
        var expectedInsurancePolicyId = 10;

        _mockInsurancePolicyRepository
            .Setup(x => x.CreateInsurancePolicyAsync(It.IsAny<CreateInsurancePolicy>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedInsurancePolicyId);
        
        // act
        var actualResponse = await _createPolicyInsuranceCommandHandler.Handle(
            newInsurancePolicy, CancellationToken.None).ConfigureAwait(false);
        
        // assert
        Assert.NotNull(actualResponse);
        Assert.Equal(expectedInsurancePolicyId, actualResponse.InsurancePolicyId);
    }
    
}