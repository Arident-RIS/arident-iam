using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Organizations;

public sealed class OrgSchemaConfiguration : IEntityTypeConfiguration<OrgSchema>
{
    public void Configure(EntityTypeBuilder<OrgSchema> builder)
    {
        builder.ToTable("OrgSchemas");

        builder.HasKey(x => x.OrgSchemaExternalId);

        builder.Property(x => x.OrgSchemaExternalId)
            .HasColumnName("OrgSchemaExternalId")
            .ValueGeneratedNever();

        builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("Description")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.IsDefault)
            .HasColumnName("IsDefault")
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

        builder.HasIndex(x => new { x.TenantExternalId, x.Name })
            .IsUnique()
            .HasDatabaseName("UX_OrgSchemas_TenantExternalId_Name");

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantExternalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}