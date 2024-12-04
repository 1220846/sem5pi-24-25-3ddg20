using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDDNetCore.Migrations
{
    /// <inheritdoc />
    public partial class FixRoomTypeForNewParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurgeryRooms_RoomTypes_RoomTypeId",
                table: "SurgeryRooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_Name",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RoomTypes");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "SurgeryRooms",
                newName: "RoomTypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_SurgeryRooms_RoomTypeId",
                table: "SurgeryRooms",
                newName: "IX_SurgeryRooms_RoomTypeCode");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Specializations",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RoomTypes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "RoomTypes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Code",
                table: "Specializations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_Designation",
                table: "RoomTypes",
                column: "Designation",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SurgeryRooms_RoomTypes_RoomTypeCode",
                table: "SurgeryRooms",
                column: "RoomTypeCode",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurgeryRooms_RoomTypes_RoomTypeCode",
                table: "SurgeryRooms");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_Code",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_RoomTypes_Designation",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "RoomTypes");

            migrationBuilder.RenameColumn(
                name: "RoomTypeCode",
                table: "SurgeryRooms",
                newName: "RoomTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SurgeryRooms_RoomTypeCode",
                table: "SurgeryRooms",
                newName: "IX_SurgeryRooms_RoomTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Specializations",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RoomTypes",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_Name",
                table: "RoomTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SurgeryRooms_RoomTypes_RoomTypeId",
                table: "SurgeryRooms",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
