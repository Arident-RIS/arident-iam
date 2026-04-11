using AridentIam.Domain.Entities.Principals;
using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Principals;

public sealed class PrincipalConfiguration : IEntityTypeConfiguration<Principal>
{
    public void Configure(EntityTypeBuilder<Principal> builder)
    {
        _ = builder.ToTable("Principals");

        _ = builder.HasKey(x => x.PrincipalExternalId);

        _ = builder.Property(x => x.PrincipalExternalId)
            .HasColumnName("PrincipalExternalId")
            .ValueGeneratedNever();

        _ = builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .IsRequired();

        _ = builder.Property(x => x.PrincipalType)
            .HasColumnName("PrincipalType")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        _ = builder.Property(x => x.DisplayName)
            .HasColumnName("DisplayName")
            .HasMaxLength(200)
            .IsRequired();

        _ = builder.Property(x => x.ExternalReference)
            .HasColumnName("ExternalReference")
            .HasMaxLength(200)
            .IsRequired(false);

        _ = builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        _ = builder.Property(x => x.LifecycleState)
            .HasColumnName("LifecycleState")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

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

        _ = builder.HasIndex(x => x.TenantExternalId)
            .HasDatabaseName("IX_Principals_TenantExternalId");

        _ = builder.HasIndex(x => new { x.TenantExternalId, x.ExternalReference })
            .IsUnique()
            .HasFilter("[ExternalReference] IS NOT NULL")
            .HasDatabaseName("UX_Principals_TenantExternalId_ExternalReference");

        _ = builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantExternalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}