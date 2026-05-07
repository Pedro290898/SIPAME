using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIPAME.Migrations
{
    /// <inheritdoc />
    public partial class basededatoslimpia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estatus",
                columns: table => new
                {
                    EstatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescripcionEstatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estatus", x => x.EstatusId);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionDomicilio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Cliente_Estatus_EstatusId",
                        column: x => x.EstatusId,
                        principalTable: "Estatus",
                        principalColumn: "EstatusId");
                });

            migrationBuilder.CreateTable(
                name: "Paquete",
                columns: table => new
                {
                    PaqueteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescripcionPaquete = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paquete", x => x.PaqueteId);
                    table.ForeignKey(
                        name: "FK_Paquete_Estatus_EstatusId",
                        column: x => x.EstatusId,
                        principalTable: "Estatus",
                        principalColumn: "EstatusId");
                });

            migrationBuilder.CreateTable(
                name: "contratacion",
                columns: table => new
                {
                    ContratacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaContratacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TpoContratacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionPaquete = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    PaqueteId = table.Column<int>(type: "int", nullable: false),
                    EstatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contratacion", x => x.ContratacionId);
                    table.ForeignKey(
                        name: "FK_contratacion_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId");
                    table.ForeignKey(
                        name: "FK_contratacion_Estatus_EstatusId",
                        column: x => x.EstatusId,
                        principalTable: "Estatus",
                        principalColumn: "EstatusId");
                    table.ForeignKey(
                        name: "FK_contratacion_Paquete_PaqueteId",
                        column: x => x.PaqueteId,
                        principalTable: "Paquete",
                        principalColumn: "PaqueteId");
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    PagoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoPago = table.Column<double>(type: "float", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ContratacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.PagoId);
                    table.ForeignKey(
                        name: "FK_Pago_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId");
                    table.ForeignKey(
                        name: "FK_Pago_contratacion_ContratacionId",
                        column: x => x.ContratacionId,
                        principalTable: "contratacion",
                        principalColumn: "ContratacionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_EstatusId",
                table: "Cliente",
                column: "EstatusId");

            migrationBuilder.CreateIndex(
                name: "IX_contratacion_ClienteId",
                table: "contratacion",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_contratacion_EstatusId",
                table: "contratacion",
                column: "EstatusId");

            migrationBuilder.CreateIndex(
                name: "IX_contratacion_PaqueteId",
                table: "contratacion",
                column: "PaqueteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_ClienteId",
                table: "Pago",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_ContratacionId",
                table: "Pago",
                column: "ContratacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Paquete_EstatusId",
                table: "Paquete",
                column: "EstatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "contratacion");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Paquete");

            migrationBuilder.DropTable(
                name: "Estatus");
        }
    }
}
