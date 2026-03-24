using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace size.Carrinho.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Carrinho");

            migrationBuilder.CreateTable(
                name: "Carrinhos",
                schema: "Carrinho",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    AgregateId = table.Column<string>(type: "varchar(200)", nullable: false),
                    InicioProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrinhos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Duplicatas",
                schema: "Carrinho",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    CarrinhoId = table.Column<string>(type: "varchar(200)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duplicatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Duplicatas_Carrinhos_CarrinhoId",
                        column: x => x.CarrinhoId,
                        principalSchema: "Carrinho",
                        principalTable: "Carrinhos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duplicatas_CarrinhoId",
                schema: "Carrinho",
                table: "Duplicatas",
                column: "CarrinhoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duplicatas",
                schema: "Carrinho");

            migrationBuilder.DropTable(
                name: "Carrinhos",
                schema: "Carrinho");
        }
    }
}
