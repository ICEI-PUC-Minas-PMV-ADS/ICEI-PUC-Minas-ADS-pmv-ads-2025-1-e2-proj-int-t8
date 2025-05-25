using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanceCertoSQL.Migrations
{
    /// <inheritdoc />
    public partial class CriatTabelaLancesCorrigida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PregaoId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lances_Pregoes_PregaoId",
                        column: x => x.PregaoId,
                        principalTable: "Pregoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lances_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lances_PregaoId",
                table: "Lances",
                column: "PregaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lances_UsuarioId",
                table: "Lances",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lances");
        }
    }
}
