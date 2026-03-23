using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace size.Operacao.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Operacao");

            migrationBuilder.CreateTable(
                name: "Operacoes",
                schema: "Operacao",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    TomadorId = table.Column<string>(type: "varchar(200)", nullable: false),
                    Codigo = table.Column<string>(type: "varchar(200)", nullable: false),
                    ValorBruto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    ValorLiquido = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TaxaAntecipacao = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Prazo = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Duplicatas",
                schema: "Operacao",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    Numero = table.Column<string>(type: "varchar(200)", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OperacaoId = table.Column<string>(type: "varchar(200)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duplicatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Duplicatas_Operacoes_OperacaoId",
                        column: x => x.OperacaoId,
                        principalSchema: "Operacao",
                        principalTable: "Operacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duplicatas_OperacaoId",
                schema: "Operacao",
                table: "Duplicatas",
                column: "OperacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duplicatas",
                schema: "Operacao");

            migrationBuilder.DropTable(
                name: "Operacoes",
                schema: "Operacao");
        }
    }
}
