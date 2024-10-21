using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDNetCore.Migrations
{
    /// <inheritdoc />
    public partial class FixPatientConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInformation",
                table: "Patients",
                newName: "PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_ContactInformation",
                table: "Patients",
                newName: "IX_Patients_PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Patients",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Patients",
                newName: "ContactInformation");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_PhoneNumber",
                table: "Patients",
                newName: "IX_Patients_ContactInformation");
        }
    }
}
