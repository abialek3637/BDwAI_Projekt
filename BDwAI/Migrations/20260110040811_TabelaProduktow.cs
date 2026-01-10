using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDwAI.Migrations
{
    /// <inheritdoc />
    public partial class TabelaProduktow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Produkts",
                type: "nchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Produkts",
                type: "nchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(50)");
        }
    }
}
