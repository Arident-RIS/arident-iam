using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AridentIam.Infrastructure.Persistence.Configurations.Users;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.UserExternalId);

        builder.Property(x => x.UserExternalId)
            .HasColumnName("UserExternalId")
            .ValueGeneratedNever();

        builder.Property(x => x.PrincipalExternalId)
            .HasColumnName("PrincipalExternalId")
            .IsRequired();

        builder.Property(x => x.TenantExternalId)
            .HasColumnName("TenantExternalId")
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.DisplayName)
            .HasColumnName("DisplayName")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(x => x.Username)
            .HasColumnName("Username")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.EmploymentType)
            .HasColumnName("EmploymentType")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.JobTitle)
            .HasColumnName("JobTitle")
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.IsEmailVerified)
            .HasColumnName("IsEmailVerified")
            .IsRequired();

        builder.Property(x => x.IsPhoneVerified)
            .HasColumnName("IsPhoneVerified")
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

        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(x => x.Username)
            .HasDatabaseName("IX_Users_Username");

        builder.HasIndex(x => new { x.TenantExternalId, x.Email })
            .IsUnique()
            .HasDatabaseName("UX_Users_TenantExternalId_Email");

        builder.HasIndex(x => new { x.TenantExternalId, x.Username })
            .IsUnique()
            .HasDatabaseName("UX_Users_TenantExternalId_Username");

        builder.HasIndex(x => x.PrincipalExternalId)
            .IsUnique()
            .HasDatabaseName("UX_Users_PrincipalExternalId");

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantExternalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}