using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDwAI.Migrations
{
    /// <inheritdoc />
    public partial class ProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Produkts",
                type: "nvchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "navchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Produkts",
                type: "navchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvchar(50)");
        }
    }
}
