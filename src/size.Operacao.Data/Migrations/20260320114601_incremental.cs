using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace size.Operacao.Data.Migrations
{
    /// <inheritdoc />
    public partial class incremental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
            name: "CodigoSequence",
            schema: "dbo",
            startValue: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                schema: "Operacao",
                table: "Operacoes",
                type: "varchar(200)",
                nullable: false,
                defaultValueSql: "CAST(NEXT VALUE FOR dbo.CodigoSequence AS VARCHAR(20))",
                oldClrType: typeof(string),
                oldType: "varchar(200)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                schema: "Operacao",
                table: "Operacoes",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldDefaultValueSql: "CAST(NEXT VALUE FOR dbo.CodigoSequence AS VARCHAR(20))");
        }
    }
}
