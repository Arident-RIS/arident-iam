using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AridentIam.Infrastructure.Persistence.Migrations.Generated
{
    /// <inheritdoc />
    public partial class InitialCreateV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Principals_Users_UserProfileUserExternalId",
                table: "Principals");

            migrationBuilder.DropIndex(
                name: "IX_Principals_UserProfileUserExternalId",
                table: "Principals");

            migrationBuilder.DropColumn(
                name: "UserProfileUserExternalId",
                table: "Principals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileUserExternalId",
                table: "Principals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_UserProfileUserExternalId",
                table: "Principals",
                column: "UserProfileUserExternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Principals_Users_UserProfileUserExternalId",
                table: "Principals",
                column: "UserProfileUserExternalId",
                principalTable: "Users",
                principalColumn: "UserExternalId");
        }
    }
}
