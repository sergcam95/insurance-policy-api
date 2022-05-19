using InsurancePolicyService.Application.Common.Interfaces;
using InsurancePolicyService.Application.Common.Interfaces.Repositories;
using InsurancePolicyService.Infrastructure.Common;
using InsurancePolicyService.Infrastructure.Persistence;
using InsurancePolicyService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsurancePolicyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
    IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // Services
        services.AddSingleton<IAddressValidator, AddressValidator>();
        services.AddSingleton<IStateRegulationService, StateRegulationService>();
        services.AddScoped<IAccountingService, AccountingService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IInsurancePolicyRepository, InsurancePolicyRepository>();
        
        // Queues
        services.AddSingleton<IAccountingNotificationQueue>(_ => 
            new AccountingNotificationQueue(20));
        
        // BackgroundTasks
        services.AddHostedService<AccountingNotificationQueueListener>();

        return services;
    }
}