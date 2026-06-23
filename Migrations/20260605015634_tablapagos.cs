using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIPAME.Migrations
{
    /// <inheritdoc />
    public partial class tablapagos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormaPago",
                table: "Pago",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TpoContratacion",
                table: "Pago",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormaPago",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "TpoContratacion",
                table: "Pago");
        }
    }
}
