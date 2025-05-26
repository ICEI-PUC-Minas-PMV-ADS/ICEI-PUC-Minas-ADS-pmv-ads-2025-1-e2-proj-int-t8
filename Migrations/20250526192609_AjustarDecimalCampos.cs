using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanceCertoSQL.Migrations
{
    /// <inheritdoc />
    public partial class AjustarDecimalCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_Usuarios_UsuarioId1",
                table: "Imoveis");

            migrationBuilder.DropIndex(
                name: "IX_Imoveis_UsuarioId1",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Imoveis");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Imoveis",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Imoveis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId1",
                table: "Imoveis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imoveis_UsuarioId1",
                table: "Imoveis",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_Usuarios_UsuarioId1",
                table: "Imoveis",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
