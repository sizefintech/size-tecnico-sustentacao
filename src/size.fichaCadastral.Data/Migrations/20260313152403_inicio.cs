using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace size.fichaCadastral.Data.Migrations
{
    /// <inheritdoc />
    public partial class inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FichaCadastral");

            migrationBuilder.CreateTable(
                name: "Tomadores",
                schema: "FichaCadastral",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    RazaoSocial = table.Column<string>(type: "varchar(200)", nullable: false),
                    Numero = table.Column<string>(type: "varchar(200)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tomadores", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tomadores",
                schema: "FichaCadastral");
        }
    }
}
