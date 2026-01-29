using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Products.Persistance.PgSQL.Migrations.Write
{
    /// <inheritdoc />
    public partial class add_nullable_publish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedOnUtc",
                table: "product_outbox",
                schema: "outbox_schema",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedOnUtc",
                table: "product_outbox",
                schema: "outbox_schema",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
