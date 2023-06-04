using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addingrealtionbetweenextracteditemsandproducttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "InvoiceItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_productId",
                table: "InvoiceItem",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Products_productId",
                table: "InvoiceItem",
                column: "productId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Products_productId",
                table: "InvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_productId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "InvoiceItem");
        }
    }
}
