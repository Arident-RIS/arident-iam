using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Organizations;

public sealed class OrgUnitTypeConfiguration : IEntityTypeConfiguration<OrgUnitType>
{
    public void Configure(EntityTypeBuilder<OrgUnitType> builder)
    {
        builder.ToTable("OrgUnitTypes");

        builder.HasKey(x => x.OrgUnitTypeExternalId);

        builder.Property(x => x.OrgUnitTypeExternalId)
            .HasColumnName("OrgUnitTypeExternalId")
            .ValueGeneratedNever();

        builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .IsRequired();

        builder.Property(x => x.OrgSchemaExternalId)
            .HasColumnName("OrgSchemaExternalId")
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("Code")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.HierarchyLevel)
            .HasColumnName("HierarchyLevel")
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
            .HasDatabaseName("UX_OrgUnitTypes_TenantExternalId_OrgSchemaExternalId_Code");

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantExternalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<OrgSchema>()
            .WithMany()
            .HasForeignKey(x => x.OrgSchemaExternalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}