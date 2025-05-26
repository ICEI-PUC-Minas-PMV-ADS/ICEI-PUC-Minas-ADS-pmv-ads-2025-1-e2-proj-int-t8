using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanceCertoSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddVencedorPregao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeVencedor",
                table: "Pregoes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioVencedorId",
                table: "Pregoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pregoes_UsuarioVencedorId",
                table: "Pregoes",
                column: "UsuarioVencedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pregoes_Usuarios_UsuarioVencedorId",
                table: "Pregoes",
                column: "UsuarioVencedorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pregoes_Usuarios_UsuarioVencedorId",
                table: "Pregoes");

            migrationBuilder.DropIndex(
                name: "IX_Pregoes_UsuarioVencedorId",
                table: "Pregoes");

            migrationBuilder.DropColumn(
                name: "NomeVencedor",
                table: "Pregoes");

            migrationBuilder.DropColumn(
                name: "UsuarioVencedorId",
                table: "Pregoes");
        }
    }
}
