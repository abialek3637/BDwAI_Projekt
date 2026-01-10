using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDwAI.Migrations
{
    /// <inheritdoc />
    public partial class Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Produkts",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Produkts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Produkts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Produkts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Produkts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Produkts",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Produkts",
                type: "nchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Produkts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
