using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIPAME.Migrations
{
    /// <inheritdoc />
    public partial class cambio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TpoContratacion",
                table: "Pago");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TpoContratacion",
                table: "Pago",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
