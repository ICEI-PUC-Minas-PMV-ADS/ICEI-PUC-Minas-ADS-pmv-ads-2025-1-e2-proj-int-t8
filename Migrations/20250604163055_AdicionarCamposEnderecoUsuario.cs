using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanceCertoSQL.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposEnderecoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CEP",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "AspNetUsers");
        }
    }
}
