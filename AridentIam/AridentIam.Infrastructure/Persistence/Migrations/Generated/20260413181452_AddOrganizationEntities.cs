using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AridentIam.Infrastructure.Persistence.Migrations.Generated
{
    /// <inheritdoc />
    public partial class AddOrganizationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrgUnits");

            migrationBuilder.DropTable(
                name: "OrgUnitTypes");

            migrationBuilder.DropTable(
                name: "OrgSchemas");
        }
    }
}
