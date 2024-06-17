using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stela_api.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CemeteryName",
                table: "PortfolioMemorials");

            migrationBuilder.AddColumn<string>(
                name: "CemeteryName",
                table: "Memorials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CemeteryName",
                table: "Memorials");

            migrationBuilder.AddColumn<string>(
                name: "CemeteryName",
                table: "PortfolioMemorials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
