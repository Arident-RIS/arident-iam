using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AridentIam.Infrastructure.Persistence.Migrations.Generated
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsolationMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultTimeZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DefaultLocale = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataResidencyRegion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantExternalId);
                });

            migrationBuilder.CreateTable(
                name: "OrgSchemas",
                columns: table => new
                {
                    OrgSchemaExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgSchemas", x => x.OrgSchemaExternalId);
                    table.ForeignKey(
                        name: "FK_OrgSchemas_Tenants_TenantExternalId",
                        column: x => x.TenantExternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantExternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantSettings",
                columns: table => new
                {
                    TenantSettingExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSettings", x => x.TenantSettingExternalId);
                    table.ForeignKey(
                        name: "FK_TenantSettings_Tenants_TenantExternalId",
                        column: x => x.TenantExternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantExternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgUnitTypes",
                columns: table => new
                {
                    OrgUnitTypeExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgSchemaExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUnitTypes", x => x.OrgUnitTypeExternalId);
                    table.ForeignKey(
                        name: "FK_OrgUnitTypes_OrgSchemas_OrgSchemaExternalId",
                        column: x => x.OrgSchemaExternalId,
                        principalTable: "OrgSchemas",
                        principalColumn: "OrgSchemaExternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgUnitTypes_Tenants_TenantExternalId",
                        column: x => x.TenantExternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantExternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgUnits",
                columns: table => new
                {
                    OrganizationUnitExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgSchemaExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgUnitTypeExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentOrganizationUnitExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUnits", x => x.OrganizationUnitExternalId);
                    table.ForeignKey(
                        name: "FK_OrgUnits_OrgSchemas_OrgSchemaExternalId",
                        column: x => x.OrgSchemaExternalId,
                        principalTable: "OrgSchemas",
                        principalColumn: "OrgSchemaExternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgUnits_OrgUnitTypes_OrgUnitTypeExternalId",
                        column: x => x.OrgUnitTypeExternalId,
                        principalTable: "OrgUnitTypes",
                        principalColumn: "OrgUnitTypeExternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgUnits_OrgUnits_ParentOrganizationUnitExternalId",
                        column: x => x.ParentOrganizationUnitExternalId,
                        principalTable: "OrgUnits",
                        principalColumn: "OrganizationUnitExternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgUnits_Tenants_TenantExternalId",
                        column: x => x.TenantExternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantExternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Principals",
                columns: table => new
                {
                    PrincipalExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrincipalType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExternalReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LifecycleState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserProfileUserExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Principals", x => x.PrincipalExternalId);
                    table.ForeignKey(
                        name: "FK_Principals_Tenants_TenantExternalId",
                        column: x => x.TenantExternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantExternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrincipalExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmploymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserExternalId);
                    table.ForeignKey(
                        name: "FK_Users_Principals_PrincipalExternalId",
                        column: x => x.PrincipalExternalId,
                        principalTable: "Principals",
                        principalColumn: "PrincipalExternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_TenantExternalId",
                        column: x => x.TenantExternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantExternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UX_OrgSchemas_TenantExternalId_Name",
                table: "OrgSchemas",
                columns: new[] { "TenantExternalId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_OrgSchemaExternalId",
                table: "OrgUnits",
                column: "OrgSchemaExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_OrgUnitTypeExternalId",
                table: "OrgUnits",
                column: "OrgUnitTypeExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_ParentOrganizationUnitExternalId",
                table: "OrgUnits",
                column: "ParentOrganizationUnitExternalId");

            migrationBuilder.CreateIndex(
                name: "UX_OrgUnits_TenantExternalId_OrgSchemaExternalId_Code",
                table: "OrgUnits",
                columns: new[] { "TenantExternalId", "OrgSchemaExternalId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnitTypes_OrgSchemaExternalId",
                table: "OrgUnitTypes",
                column: "OrgSchemaExternalId");

            migrationBuilder.CreateIndex(
                name: "UX_OrgUnitTypes_TenantExternalId_OrgSchemaExternalId_Code",
                table: "OrgUnitTypes",
                columns: new[] { "TenantExternalId", "OrgSchemaExternalId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_TenantExternalId",
                table: "Principals",
                column: "TenantExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Principals_UserProfileUserExternalId",
                table: "Principals",
                column: "UserProfileUserExternalId");

            migrationBuilder.CreateIndex(
                name: "UX_Principals_TenantExternalId_ExternalReference",
                table: "Principals",
                columns: new[] { "TenantExternalId", "ExternalReference" },
                unique: true,
                filter: "[ExternalReference] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "UX_Tenants_Code",
                table: "Tenants",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_TenantSettings_TenantExternalId_SettingKey",
                table: "TenantSettings",
                columns: new[] { "TenantExternalId", "SettingKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "UX_Users_PrincipalExternalId",
                table: "Users",
                column: "PrincipalExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Users_TenantExternalId_Email",
                table: "Users",
                columns: new[] { "TenantExternalId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Users_TenantExternalId_Username",
                table: "Users",
                columns: new[] { "TenantExternalId", "Username" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Principals_Users_UserProfileUserExternalId",
                table: "Principals",
                column: "UserProfileUserExternalId",
                principalTable: "Users",
                principalColumn: "UserExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Principals_Tenants_TenantExternalId",
                table: "Principals");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantExternalId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Principals_Users_UserProfileUserExternalId",
                table: "Principals");

            migrationBuilder.DropTable(
                name: "OrgUnits");

            migrationBuilder.DropTable(
                name: "TenantSettings");

            migrationBuilder.DropTable(
                name: "OrgUnitTypes");

            migrationBuilder.DropTable(
                name: "OrgSchemas");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Principals");
        }
    }
}
