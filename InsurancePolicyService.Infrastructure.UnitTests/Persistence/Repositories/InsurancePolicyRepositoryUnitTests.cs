using System.Threading;
using System.Threading.Tasks;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Application.Common.Models.Repositories;
using InsurancePolicyService.Domain;
using InsurancePolicyService.Infrastructure.Persistence;
using InsurancePolicyService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace InsurancePolicyService.Infrastructure.UnitTests.Persistence.Repositories;

public class InsurancePolicyRepositoryUnitTests
{
    private readonly Mock<ApplicationDbContext> _mockApplicationDbContext;
    private readonly Mock<IAddressRepository> _mockAddressRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;

    private readonly InsurancePolicyRepository _insurancePolicyRepository;

    public InsurancePolicyRepositoryUnitTests()
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        _mockApplicationDbContext = new Mock<ApplicationDbContext>(options);
        _mockAddressRepository = new Mock<IAddressRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockVehicleRepository = new Mock<IVehicleRepository>();

        var mockIDbContextTransaction = new Mock<IDbContextTransaction>();
        var mockDbContextDatabase = new Mock<DatabaseFacade>(_mockApplicationDbContext.Object);
        mockDbContextDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockIDbContextTransaction.Object);
        
        _mockApplicationDbContext
            .Setup(x => x.Database)
            .Returns(mockDbContextDatabase.Object);
        
        _mockUserRepository
            .Setup(x => x.GetUserByDriversLicenseNumberAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                UserID = 1
            });

        _mockAddressRepository
            .Setup(x => x.GetAddressByFullAddressNameAsync(It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Address
            {
                AddressID = 1
            });
        
        _mockVehicleRepository
            .Setup(x => x.GetVehicleByNameAsync(It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Vehicle
            {
                VehicleID = 1
            });

        _insurancePolicyRepository = new InsurancePolicyRepository(
            _mockApplicationDbContext.Object,
            _mockAddressRepository.Object,
            _mockUserRepository.Object,
            _mockVehicleRepository.Object);
    }

    [Fact]
    public async Task
        CreateInsurancePolicyAsync_ShouldUseExistingUser_WhenDriversLicenseMatchesAnExistingRecord()
    {
        // arrange
        var fakeInsurancePolicy = new CreateInsurancePolicy
        {
            User = new CreateUser(),
            Address = new CreateAddress(),
            Vehicle = new CreateVehicle()
        };

        var expectedUserID = 2;
        _mockUserRepository
            .Setup(x => x.GetUserByDriversLicenseNumberAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                UserID = expectedUserID
            });

        var createdInsurancePolicy = CreateEntityEntry(new InsurancePolicy
        {
            InsurancePolicyID = 1
        });

        var actualSavedInsurancePolicy = (InsurancePolicy)null;
        var mockInsurancePolicyDbSet = new Mock<DbSet<InsurancePolicy>>();
        mockInsurancePolicyDbSet
            .Setup(x =>
                x.AddAsync(It.IsAny<InsurancePolicy>(),
                    It.IsAny<CancellationToken>()))
            .Callback<InsurancePolicy, CancellationToken>((insurancePolicy, _) =>
                actualSavedInsurancePolicy = insurancePolicy)
            .ReturnsAsync(createdInsurancePolicy);

        _mockApplicationDbContext
            .Setup(x =>
                x.InsurancePolicies)
            .Returns(mockInsurancePolicyDbSet.Object);

        // act
        await _insurancePolicyRepository.CreateInsurancePolicyAsync(fakeInsurancePolicy, 
            CancellationToken.None).ConfigureAwait(false);

        // assert
        Assert.Equal(expectedUserID, actualSavedInsurancePolicy!.UserID);
    }
    
    [Fact]
    public async Task
        CreateInsurancePolicyAsync_ShouldUseExistingAddress_WhenFullNameAndUserIDMatchAnExistingRecord()
    {
        // arrange
        var fakeInsurancePolicy = new CreateInsurancePolicy
        {
            User = new CreateUser(),
            Address = new CreateAddress(),
            Vehicle = new CreateVehicle()
        };

        var expectedAddressID = 2;
        _mockAddressRepository
            .Setup(x => x.GetAddressByFullAddressNameAsync(It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Address
            {
                AddressID = expectedAddressID
            });

        var createdInsurancePolicy = CreateEntityEntry(new InsurancePolicy
        {
            InsurancePolicyID = 1
        });

        var actualSavedInsurancePolicy = (InsurancePolicy)null;
        var mockInsurancePolicyDbSet = new Mock<DbSet<InsurancePolicy>>();
        mockInsurancePolicyDbSet
            .Setup(x =>
                x.AddAsync(It.IsAny<InsurancePolicy>(),
                    It.IsAny<CancellationToken>()))
            .Callback<InsurancePolicy, CancellationToken>((insurancePolicy, _) =>
                actualSavedInsurancePolicy = insurancePolicy)
            .ReturnsAsync(createdInsurancePolicy);

        _mockApplicationDbContext
            .Setup(x =>
                x.InsurancePolicies)
            .Returns(mockInsurancePolicyDbSet.Object);

        // act
        await _insurancePolicyRepository.CreateInsurancePolicyAsync(fakeInsurancePolicy, 
            CancellationToken.None).ConfigureAwait(false);

        // assert
        Assert.Equal(expectedAddressID, actualSavedInsurancePolicy!.AddressID);
    }
    
    [Fact]
    public async Task
        CreateInsurancePolicyAsync_ShouldUseExistingVehicle_WhenVehicleNameAndUserIDMatchAnExistingRecord()
    {
        // arrange
        var fakeInsurancePolicy = new CreateInsurancePolicy
        {
            User = new CreateUser(),
            Address = new CreateAddress(),
            Vehicle = new CreateVehicle()
        };

        var expectedVehicleID = 2;
        _mockVehicleRepository
            .Setup(x => x.GetVehicleByNameAsync(It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Vehicle
            {
                VehicleID = expectedVehicleID
            });

        var createdInsurancePolicy = CreateEntityEntry(new InsurancePolicy
        {
            InsurancePolicyID = 1
        });

        var actualSavedInsurancePolicy = (InsurancePolicy)null;
        var mockInsurancePolicyDbSet = new Mock<DbSet<InsurancePolicy>>();
        mockInsurancePolicyDbSet
            .Setup(x =>
                x.AddAsync(It.IsAny<InsurancePolicy>(),
                    It.IsAny<CancellationToken>()))
            .Callback<InsurancePolicy, CancellationToken>((insurancePolicy, _) =>
                actualSavedInsurancePolicy = insurancePolicy)
            .ReturnsAsync(createdInsurancePolicy);

        _mockApplicationDbContext
            .Setup(x =>
                x.InsurancePolicies)
            .Returns(mockInsurancePolicyDbSet.Object);

        // act
        await _insurancePolicyRepository.CreateInsurancePolicyAsync(fakeInsurancePolicy, 
            CancellationToken.None).ConfigureAwait(false);

        // assert
        Assert.Equal(expectedVehicleID, actualSavedInsurancePolicy!.VehicleID);
    }
    
    [Fact]
    public async Task
        CreateInsurancePolicyAsync_ShouldCreateNewAddress_WheFullNameAndUserIDDoNotMatchAnExistingRecord()
    {
        // arrange
        var fakeInsurancePolicy = new CreateInsurancePolicy
        {
            User = new CreateUser(),
            Address = new CreateAddress(),
            Vehicle = new CreateVehicle()
        };

        var expectedNewAddressID = 2;
        _mockAddressRepository
            .Setup(x => x.CreateAddressAsync(It.IsAny<CreateAddress>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNewAddressID);
        _mockAddressRepository
            .Setup(x => x.GetAddressByFullAddressNameAsync(It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Address)null);

        var createdInsurancePolicy = CreateEntityEntry(new InsurancePolicy
        {
            InsurancePolicyID = 1
        });

        var actualSavedInsurancePolicy = (InsurancePolicy)null;
        var mockInsurancePolicyDbSet = new Mock<DbSet<InsurancePolicy>>();
        mockInsurancePolicyDbSet
            .Setup(x =>
                x.AddAsync(It.IsAny<InsurancePolicy>(),
                    It.IsAny<CancellationToken>()))
            .Callback<InsurancePolicy, CancellationToken>((insurancePolicy, _) =>
                actualSavedInsurancePolicy = insurancePolicy)
            .ReturnsAsync(createdInsurancePolicy);

        _mockApplicationDbContext
            .Setup(x =>
                x.InsurancePolicies)
            .Returns(mockInsurancePolicyDbSet.Object);

        // act
        await _insurancePolicyRepository.CreateInsurancePolicyAsync(fakeInsurancePolicy, 
            CancellationToken.None).ConfigureAwait(false);

        // assert
        Assert.Equal(expectedNewAddressID, actualSavedInsurancePolicy!.AddressID);
    }
    
    [Fact]
    public async Task
        CreateInsurancePolicyAsync_ShouldCreateNewVehicle_WhenVehicleNameAndUserIDDoNotMatchAnExistingRecord()
    {
        // arrange
        var fakeInsurancePolicy = new CreateInsurancePolicy
        {
            User = new CreateUser(),
            Address = new CreateAddress(),
            Vehicle = new CreateVehicle()
        };

        var expectedNewVehicleID = 2;
        _mockVehicleRepository
            .Setup(x => x.CreateVehicleAsync(It.IsAny<CreateVehicle>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNewVehicleID);
        _mockVehicleRepository
            .Setup(x => x.GetVehicleByNameAsync(It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle)null);

        var createdInsurancePolicy = CreateEntityEntry(new InsurancePolicy
        {
            InsurancePolicyID = 1
        });

        var actualSavedInsurancePolicy = (InsurancePolicy)null;
        var mockInsurancePolicyDbSet = new Mock<DbSet<InsurancePolicy>>();
        mockInsurancePolicyDbSet
            .Setup(x =>
                x.AddAsync(It.IsAny<InsurancePolicy>(),
                    It.IsAny<CancellationToken>()))
            .Callback<InsurancePolicy, CancellationToken>((insurancePolicy, _) =>
                actualSavedInsurancePolicy = insurancePolicy)
            .ReturnsAsync(createdInsurancePolicy);

        _mockApplicationDbContext
            .Setup(x =>
                x.InsurancePolicies)
            .Returns(mockInsurancePolicyDbSet.Object);

        // act
        await _insurancePolicyRepository.CreateInsurancePolicyAsync(fakeInsurancePolicy, 
            CancellationToken.None).ConfigureAwait(false);

        // assert
        Assert.Equal(expectedNewVehicleID, actualSavedInsurancePolicy!.VehicleID);
    }

    private EntityEntry<T> CreateEntityEntry<T>(T model) where T : class
    {
        return new EntityEntry<T>(new InternalEntityEntry(
            new Mock<IStateManager>().Object,
            new RuntimeEntityType(nameof(T), typeof(T), false, null, null, null, 
                ChangeTrackingStrategy.Snapshot, null, false),
            model));
    }
}