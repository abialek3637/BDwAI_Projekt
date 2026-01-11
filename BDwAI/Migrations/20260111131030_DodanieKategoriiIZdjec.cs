using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDwAI.Migrations
{
    /// <inheritdoc />
    public partial class DodanieKategoriiIZdjec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Produkts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Produkts");
        }
    }
}
