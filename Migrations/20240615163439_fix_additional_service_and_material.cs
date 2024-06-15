using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stela_api.Migrations
{
    /// <inheritdoc />
    public partial class fix_additional_service_and_material : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AdditionalServices");

            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                table: "Materials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorName",
                table: "Materials");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AdditionalServices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
