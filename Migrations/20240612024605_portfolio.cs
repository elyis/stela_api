using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stela_api.Migrations
{
    /// <inheritdoc />
    public partial class portfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortfolioMemorials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    CemeteryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioMemorials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioMemorialMaterials",
                columns: table => new
                {
                    PortfolioMemorialId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemorialMaterialId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioMemorialMaterials", x => new { x.MemorialMaterialId, x.PortfolioMemorialId });
                    table.ForeignKey(
                        name: "FK_PortfolioMemorialMaterials_Materials_MemorialMaterialId",
                        column: x => x.MemorialMaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioMemorialMaterials_PortfolioMemorials_PortfolioMemo~",
                        column: x => x.PortfolioMemorialId,
                        principalTable: "PortfolioMemorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioMemorialMaterials_PortfolioMemorialId",
                table: "PortfolioMemorialMaterials",
                column: "PortfolioMemorialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioMemorialMaterials");

            migrationBuilder.DropTable(
                name: "PortfolioMemorials");
        }
    }
}
