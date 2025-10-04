using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webport.ERP.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatev1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_items_item_code",
                schema: "inventory",
                table: "items");

            migrationBuilder.DropIndex(
                name: "ix_categories_category_code",
                schema: "inventory",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "ix_items_tenant_id_item_code",
                schema: "inventory",
                table: "items",
                columns: ["tenant_id", "item_code"],
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_tenant_id_category_code",
                schema: "inventory",
                table: "categories",
                columns: ["tenant_id", "category_code"],
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_items_tenant_id_item_code",
                schema: "inventory",
                table: "items");

            migrationBuilder.DropIndex(
                name: "ix_categories_tenant_id_category_code",
                schema: "inventory",
                table: "categories");

            migrationBuilder.CreateIndex(
                name: "ix_items_item_code",
                schema: "inventory",
                table: "items",
                column: "item_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_category_code",
                schema: "inventory",
                table: "categories",
                column: "category_code",
                unique: true);
        }
    }
}
