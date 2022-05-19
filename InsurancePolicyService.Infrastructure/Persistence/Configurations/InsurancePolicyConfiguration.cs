using InsurancePolicyService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsurancePolicyService.Infrastructure.Persistence.Configurations;

public class InsurancePolicyConfiguration : IEntityTypeConfiguration<InsurancePolicy>
{
    public void Configure(EntityTypeBuilder<InsurancePolicy> builder)
    {
        builder.ToTable("InsurancePolicy");

        builder.Property<decimal>(e => e.Premium)
            .HasPrecision(6, 2);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserID)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne<Address>()
            .WithMany()
            .HasForeignKey(e => e.AddressID)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(e => e.VehicleID)
            .OnDelete(DeleteBehavior.NoAction);
    }
}