using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Organizations;

public sealed class OrgUnitConfiguration : IEntityTypeConfiguration<OrgUnit>
{
    public void Configure(EntityTypeBuilder<OrgUnit> builder)
    {
        builder.ToTable("OrgUnits");

        builder.HasKey(x => x.OrganizationUnitExternalId);

        builder.Property(x => x.OrganizationUnitExternalId)
            .HasColumnName("OrganizationUnitExternalId")
            .ValueGeneratedNever();

        builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .IsRequired();

        builder.Property(x => x.OrgSchemaExternalId)
            .HasColumnName("OrgSchemaExternalId")
            .IsRequired();

        builder.Property(x => x.OrgUnitTypeExternalId)
            .HasColumnName("OrgUnitTypeExternalId")
            .IsRequired();

        builder.Property(x => x.ParentOrganizationUnitExternalId)
            .HasColumnName("ParentOrganizationUnitExternalId")
            .IsRequired(false);

        builder.Property(x => x.Code)
            .HasColumnName("Code")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Path)
            .HasColumnName("Path")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Depth)
            .HasColumnName("Depth")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasMaxLength(50)
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

        builder.HasIndex(x => new { x.TenantExternalId, x.OrgSchemaExternalId, x.Code })
            .IsUnique()
            .HasDatabaseName("UX_OrgUnits_TenantExternalId_OrgSchemaExternalId_Code");

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantExternalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<OrgSchema>()
            .WithMany()
            .HasForeignKey(x => x.OrgSchemaExternalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<OrgUnitType>()
            .WithMany()
            .HasForeignKey(x => x.OrgUnitTypeExternalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<OrgUnit>()
            .WithMany()
            .HasForeignKey(x => x.ParentOrganizationUnitExternalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}