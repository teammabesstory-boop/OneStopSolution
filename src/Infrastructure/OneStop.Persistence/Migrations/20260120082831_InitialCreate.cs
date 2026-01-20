using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OneStop.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Unit_Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Unit_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Unit_Factor = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MinimumStock = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CurrentPricePerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LastPriceUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OutputQuantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OutputUnit_Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OutputUnit_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OutputUnit_Factor = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CachedCostPerUnit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LastCostCalculation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMutations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Unit_Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Unit_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Unit_Factor = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ReferenceDocument = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMutations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItems",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    Unit_Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Unit_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Unit_Factor = table.Column<decimal>(type: "numeric(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItems", x => new { x.RecipeId, x.Id });
                    table.ForeignKey(
                        name: "FK_RecipeItems_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Sku",
                table: "Ingredients",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMutations_ItemId",
                table: "StockMutations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMutations_Timestamp",
                table: "StockMutations",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "RecipeItems");

            migrationBuilder.DropTable(
                name: "StockMutations");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
