using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Products.Persistance.PgSQL.Migrations.Write
{
    /// <inheritdoc />
    public partial class remove_foreign_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_CategoryId",
                schema: "product_schema",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_CategoryId",
                schema: "product_schema",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryId",
                schema: "product_schema",
                table: "products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_CategoryId",
                schema: "product_schema",
                table: "products",
                column: "CategoryId",
                principalSchema: "category_schema",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
