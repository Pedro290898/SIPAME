using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIPAME.Migrations
{
    /// <inheritdoc />
    public partial class tablazona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zona",
                table: "Cliente");

            migrationBuilder.AddColumn<int>(
                name: "ZonaId",
                table: "Cliente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Zona",
                columns: table => new
                {
                    ZonaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescripcionZona = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zona", x => x.ZonaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_ZonaId",
                table: "Cliente",
                column: "ZonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Zona_ZonaId",
                table: "Cliente",
                column: "ZonaId",
                principalTable: "Zona",
                principalColumn: "ZonaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Zona_ZonaId",
                table: "Cliente");

            migrationBuilder.DropTable(
                name: "Zona");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_ZonaId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "ZonaId",
                table: "Cliente");

            migrationBuilder.AddColumn<string>(
                name: "Zona",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
