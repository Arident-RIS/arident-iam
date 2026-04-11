using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Tenants;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(x => x.TenantExternalId);

        builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .ValueGeneratedNever();

        builder.Property(x => x.Code)
            .HasColumnName("Code")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IsolationMode)
            .HasColumnName("IsolationMode")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DefaultTimeZone)
            .HasColumnName("DefaultTimeZone")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.DefaultLocale)
            .HasColumnName("DefaultLocale")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.DataResidencyRegion)
            .HasColumnName("DataResidencyRegion")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAtUtc")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAtUtc")
            .IsRequired(false);

        builder.Property(x => x.UpdatedBy)
            .HasColumnName("UpdatedBy")
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("UX_Tenants_Code");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Tenants_Name");

        builder.HasMany(x => x.Settings)
            .WithOne()
            .HasForeignKey(x => x.TenantExternalId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata
            .FindNavigation(nameof(Tenant.Settings))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}