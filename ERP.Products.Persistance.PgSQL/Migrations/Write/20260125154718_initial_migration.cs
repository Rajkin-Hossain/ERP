using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Products.Persistance.PgSQL.Migrations.PgDbContext
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "category_schema");

            migrationBuilder.EnsureSchema(
                name: "outbox_schema");

            migrationBuilder.EnsureSchema(
                name: "product_schema");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "category_schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_outbox",
                schema: "outbox_schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PublishedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_outbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "product_schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "category_schema",
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryId",
                schema: "product_schema",
                table: "products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_outbox",
                schema: "outbox_schema");

            migrationBuilder.DropTable(
                name: "products",
                schema: "product_schema");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "category_schema");
        }
    }
}
