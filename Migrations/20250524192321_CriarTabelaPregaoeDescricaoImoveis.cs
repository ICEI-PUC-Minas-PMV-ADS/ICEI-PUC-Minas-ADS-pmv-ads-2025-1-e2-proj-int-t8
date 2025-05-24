using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanceCertoSQL.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaPregaoeDescricaoImoveis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
  
            // Cria a tabela Pregoes
            migrationBuilder.CreateTable(
                name: "Pregoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImovelId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ValorMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pregoes_Imoveis_ImovelId",
                        column: x => x.ImovelId,
                        principalTable: "Imoveis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pregoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pregoes_ImovelId",
                table: "Pregoes",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pregoes_UsuarioId",
                table: "Pregoes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pregoes");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Imoveis");
        }
    }
}
