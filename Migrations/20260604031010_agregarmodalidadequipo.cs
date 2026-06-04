using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIPAME.Migrations
{
    /// <inheritdoc />
    public partial class agregarmodalidadequipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Modocontratacion",
                table: "contratacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modocontratacion",
                table: "contratacion");
        }
    }
}
