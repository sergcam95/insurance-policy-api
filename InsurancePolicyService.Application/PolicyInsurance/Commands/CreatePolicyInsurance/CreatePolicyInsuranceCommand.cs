using AutoMapper;
using FluentValidation;
using InsurancePolicyService.Application.Common.Exceptions;
using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models;
using InsurancePolicyService.Application.Common.Models.Messages;
using InsurancePolicyService.Application.Common.Models.Repositories;
using MediatR;

namespace InsurancePolicyService.Application.PolicyInsurance.Commands.CreatePolicyInsurance;

public class CreatePolicyInsuranceCommand : IRequest<CreatePolicyInsuranceDto>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DriversLicenseNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    
    public int VehicleYear { get; set; }
    public string VehicleModel { get; set; } = null!;
    public string VehicleManufacturer { get; set; } = null!;
    public string VehicleName { get; set; } = null!;
    
    public DateTime EffectiveDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public decimal Premium { get; set; }
}

public class CreatePolicyInsuranceCommandValidator : AbstractValidator<CreatePolicyInsuranceCommand>
{
    public CreatePolicyInsuranceCommandValidator()
    {
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
        RuleFor(e => e.DriversLicenseNumber).NotEmpty();
        RuleFor(e => e.Address).NotEmpty();

        // Not working
        RuleFor(e => e.VehicleYear)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(e => e.VehicleModel).NotEmpty();
        RuleFor(e => e.VehicleManufacturer).NotEmpty();
        RuleFor(e => e.VehicleName).NotEmpty();

        RuleFor(e => e.EffectiveDate).NotEmpty();
        RuleFor(e => e.ExpirationDate).NotEmpty();
        RuleFor(e => e.Premium).GreaterThan(0);
    }
}

public class
    CreatePolicyInsuranceCommandHandler : IRequestHandler<CreatePolicyInsuranceCommand,
        CreatePolicyInsuranceDto>
{
    private readonly IAddressValidator _addressValidator;
    private readonly IStateRegulationService _stateRegulationService;
    private readonly IInsurancePolicyRepository _insurancePolicyRepository;
    private readonly IAccountingNotificationQueue _accountingNotificationQueue;
    private readonly IMapper _mapper;

    public CreatePolicyInsuranceCommandHandler(
        IAddressValidator addressValidator,
        IStateRegulationService stateRegulationService,
        IInsurancePolicyRepository insurancePolicyRepository,
        IAccountingNotificationQueue accountingNotificationQueue,
        IMapper mapper)
    {
        _addressValidator = addressValidator;
        _stateRegulationService = stateRegulationService;
        _insurancePolicyRepository = insurancePolicyRepository;
        _accountingNotificationQueue = accountingNotificationQueue;
        _mapper = mapper;
    }
    
    public async Task<CreatePolicyInsuranceDto> Handle(CreatePolicyInsuranceCommand request, 
    CancellationToken cancellationToken)
    {
        var creationDateTime = DateTime.UtcNow;

        #region Business rules

        if (request.VehicleYear <= 0)
            throw new RequestValidationException("Year is invalid. It should be greater than 0");

        var addressValidationResult =
            await _addressValidator.ValidateAddressAsync(request.Address, cancellationToken).ConfigureAwait(false);

        if (!addressValidationResult.IsValid)
            throw new RequestValidationException(addressValidationResult.ErrorMessage!);

        if (request.EffectiveDate - creationDateTime < TimeSpan.FromDays(30))
            throw new RequestValidationException("Effective Date should be at least 30 days after creation date");
        
        if (request.VehicleYear >= 1998)
            throw new RequestValidationException("Not a classic vehicle");

        if (request.ExpirationDate < request.EffectiveDate)
            throw new RequestValidationException("ExpirationDate should be after EffectiveDate");

        #endregion

        var stateRegulationValidationResult = _stateRegulationService.ValidateInsurancePolicy(
            _mapper.Map<InsurancePolicyStateRegulation>(request), cancellationToken);
        
        if (!stateRegulationValidationResult.IsValid)
            throw new RequestValidationException(stateRegulationValidationResult.ErrorMessage!);
        
        var insurancePolicyId = await _insurancePolicyRepository.CreateInsurancePolicyAsync(
            new CreateInsurancePolicy
            {
                EffectiveDate = request.EffectiveDate,
                ExpirationDate = request.ExpirationDate,
                Premium = request.Premium,

                Vehicle = new CreateVehicle
                {
                    Name = request.VehicleName,
                    Model = request.VehicleModel,
                    Year = request.VehicleYear,
                    Manufacturer = request.VehicleManufacturer,
                },
                User = new CreateUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DriversLicenseNumber = request.DriversLicenseNumber
                },
                Address = new CreateAddress
                {
                    FullAddress = request.Address
                }
            }, cancellationToken).ConfigureAwait(false);

        // Not awaiting this action
        _accountingNotificationQueue.QueueBackgroundWorkItemAsync(
            new InsurancePolicyCreationMessage
            {
                InsurancePolicyID = insurancePolicyId
            });
        
        return new CreatePolicyInsuranceDto
        {
            InsurancePolicyId = insurancePolicyId
        };
    }
}