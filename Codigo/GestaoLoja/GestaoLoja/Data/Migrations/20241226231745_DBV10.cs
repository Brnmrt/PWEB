using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoLoja.Migrations
{
    /// <inheritdoc />
    public partial class DBV10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensEncomenda_Encomendas_EncomendaId",
                table: "ItensEncomenda");

            migrationBuilder.DropIndex(
                name: "IX_ItensEncomenda_EncomendaId",
                table: "ItensEncomenda");

            migrationBuilder.DropColumn(
                name: "EncomendaId",
                table: "ItensEncomenda");

            migrationBuilder.RenameColumn(
                name: "Enviadas",
                table: "Encomendas",
                newName: "Enviada");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "Encomendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "Encomendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Encomendas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_ProdutoId",
                table: "Encomendas",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_UserId",
                table: "Encomendas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Encomendas_AspNetUsers_UserId",
                table: "Encomendas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Encomendas_Produtos_ProdutoId",
                table: "Encomendas",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encomendas_AspNetUsers_UserId",
                table: "Encomendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Encomendas_Produtos_ProdutoId",
                table: "Encomendas");

            migrationBuilder.DropIndex(
                name: "IX_Encomendas_ProdutoId",
                table: "Encomendas");

            migrationBuilder.DropIndex(
                name: "IX_Encomendas_UserId",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Encomendas");

            migrationBuilder.RenameColumn(
                name: "Enviada",
                table: "Encomendas",
                newName: "Enviadas");

            migrationBuilder.AddColumn<int>(
                name: "EncomendaId",
                table: "ItensEncomenda",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensEncomenda_EncomendaId",
                table: "ItensEncomenda",
                column: "EncomendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensEncomenda_Encomendas_EncomendaId",
                table: "ItensEncomenda",
                column: "EncomendaId",
                principalTable: "Encomendas",
                principalColumn: "Id");
        }
    }
}
