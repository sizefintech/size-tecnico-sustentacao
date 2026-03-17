using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace size.CatalogoRecebiveis.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CatalogoRecebiveis");

            migrationBuilder.CreateTable(
                name: "Duplicatas",
                schema: "CatalogoRecebiveis",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    TomadorId = table.Column<string>(type: "varchar(200)", nullable: false),
                    Numero = table.Column<string>(type: "varchar(200)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ValorLiquido = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NoCarrinho = table.Column<bool>(type: "bit", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duplicatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duplicatas",
                schema: "CatalogoRecebiveis");
        }
    }
}
