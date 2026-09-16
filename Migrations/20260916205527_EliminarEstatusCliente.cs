using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIPAME.Migrations
{
    /// <inheritdoc />
    public partial class EliminarEstatusCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Estatus_EstatusId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_EstatusId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "EstatusId",
                table: "Cliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstatusId",
                table: "Cliente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_EstatusId",
                table: "Cliente",
                column: "EstatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Estatus_EstatusId",
                table: "Cliente",
                column: "EstatusId",
                principalTable: "Estatus",
                principalColumn: "EstatusId");
        }
    }
}
