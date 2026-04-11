using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Tenants;

public sealed class TenantSettingConfiguration : IEntityTypeConfiguration<TenantSetting>
{
    public void Configure(EntityTypeBuilder<TenantSetting> builder)
    {
        builder.ToTable("TenantSettings");

        builder.HasKey(x => x.TenantSettingExternalId);

        builder.Property(x => x.TenantSettingExternalId)
            .HasColumnName("TenantSettingExternalId")
            .ValueGeneratedNever();

        builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .IsRequired();

        builder.Property(x => x.SettingKey)
            .HasColumnName("SettingKey")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.SettingValue)
            .HasColumnName("SettingValue")
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.Category)
            .HasColumnName("Category")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IsSensitive)
            .HasColumnName("IsSensitive")
            .IsRequired();

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

        builder.HasIndex(x => new { x.TenantExternalId, x.SettingKey })
            .IsUnique()
            .HasDatabaseName("UX_TenantSettings_TenantExternalId_SettingKey");
    }
}
