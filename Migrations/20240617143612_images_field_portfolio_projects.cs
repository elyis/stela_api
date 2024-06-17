using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stela_api.Migrations
{
    /// <inheritdoc />
    public partial class images_field_portfolio_projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "PortfolioMemorials",
                newName: "Images");

            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Memorials",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Images",
                table: "PortfolioMemorials",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Memorials",
                newName: "Images");
        }
    }
}
