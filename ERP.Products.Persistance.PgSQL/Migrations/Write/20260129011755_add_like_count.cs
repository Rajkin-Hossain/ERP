using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Products.Persistance.PgSQL.Migrations.Write
{
    /// <inheritdoc />
    public partial class add_like_count : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AggregateId",
                schema: "outbox_schema",
                table: "product_outbox",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AggregateId",
                schema: "outbox_schema",
                table: "product_outbox",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
