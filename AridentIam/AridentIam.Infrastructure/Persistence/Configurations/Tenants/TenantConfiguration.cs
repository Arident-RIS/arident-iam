using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Tenants;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        _ = builder.ToTable("Tenants");

        _ = builder.HasKey(x => x.TenantExternalId);

        _ = builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .ValueGeneratedNever();

        _ = builder.Property(x => x.Code)
            .HasColumnName("Code")
            .HasMaxLength(100)
            .IsRequired();

        _ = builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        _ = builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        _ = builder.Property(x => x.IsolationMode)
            .HasColumnName("IsolationMode")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        _ = builder.Property(x => x.DefaultTimeZone)
            .HasColumnName("DefaultTimeZone")
            .HasMaxLength(100)
            .IsRequired(false);

        _ = builder.Property(x => x.DefaultLocale)
            .HasColumnName("DefaultLocale")
            .HasMaxLength(20)
            .IsRequired(false);

        _ = builder.Property(x => x.DataResidencyRegion)
            .HasColumnName("DataResidencyRegion")
            .HasMaxLength(100)
            .IsRequired(false);

        _ = builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAtUtc")
            .IsRequired();

        _ = builder.Property(x => x.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(200)
            .IsRequired(false);

        _ = builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAtUtc")
            .IsRequired(false);

        _ = builder.Property(x => x.UpdatedBy)
            .HasColumnName("UpdatedBy")
            .HasMaxLength(200)
            .IsRequired(false);

        _ = builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("UX_Tenants_Code");

        _ = builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Tenants_Name");

        // Map the private backing field _settings for the TenantSetting collection.
        _ = builder.HasMany<TenantSetting>("_settings")
            .WithOne()
            .HasForeignKey(x => x.TenantExternalId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}