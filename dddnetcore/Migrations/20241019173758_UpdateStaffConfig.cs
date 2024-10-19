using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDNetCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaffConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInformation",
                table: "Staffs",
                newName: "PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_ContactInformation",
                table: "Staffs",
                newName: "IX_Staffs_PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Staffs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Staffs");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Staffs",
                newName: "ContactInformation");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_PhoneNumber",
                table: "Staffs",
                newName: "IX_Staffs_ContactInformation");
        }
    }
}
