using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stela_api.Migrations
{
    /// <inheritdoc />
    public partial class busket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPasswordDateModified",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Buskets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buskets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Hex = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Memorials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CemeteryName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Images = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memorials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusketItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemorialId = table.Column<Guid>(type: "uuid", nullable: false),
                    BusketId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusketItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusketItems_Buskets_BusketId",
                        column: x => x.BusketId,
                        principalTable: "Buskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusketItems_Memorials_MemorialId",
                        column: x => x.MemorialId,
                        principalTable: "Memorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemorialMaterials",
                columns: table => new
                {
                    MemorialId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemorialMaterials", x => new { x.MemorialId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_MemorialMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemorialMaterials_Memorials_MemorialId",
                        column: x => x.MemorialId,
                        principalTable: "Memorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusketItems_BusketId",
                table: "BusketItems",
                column: "BusketId");

            migrationBuilder.CreateIndex(
                name: "IX_BusketItems_MemorialId",
                table: "BusketItems",
                column: "MemorialId");

            migrationBuilder.CreateIndex(
                name: "IX_Buskets_AccountId",
                table: "Buskets",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Name",
                table: "Materials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemorialMaterials_MaterialId",
                table: "MemorialMaterials",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusketItems");

            migrationBuilder.DropTable(
                name: "MemorialMaterials");

            migrationBuilder.DropTable(
                name: "Buskets");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Memorials");

            migrationBuilder.DropColumn(
                name: "LastPasswordDateModified",
                table: "Accounts");
        }
    }
}
