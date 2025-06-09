using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentControlApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPrecoAtualToPosicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "preco_atual",
                table: "posicoes",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preco_atual",
                table: "posicoes");
        }
    }
}
