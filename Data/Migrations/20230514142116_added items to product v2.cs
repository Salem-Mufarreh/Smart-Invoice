using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addeditemstoproductv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_ProductInvoice_Product_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_Product_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "Product_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.AddColumn<int>(
                name: "ProductInvoiceId",
                table: "InvoiceItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_ProductInvoiceId",
                table: "InvoiceItem",
                column: "ProductInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_ProductInvoice_ProductInvoiceId",
                table: "InvoiceItem",
                column: "ProductInvoiceId",
                principalTable: "ProductInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_ProductInvoice_ProductInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_ProductInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "ProductInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.AddColumn<int>(
                name: "Product_InvoiceId",
                table: "InvoiceItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_Product_InvoiceId",
                table: "InvoiceItem",
                column: "Product_InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_ProductInvoice_Product_InvoiceId",
                table: "InvoiceItem",
                column: "Product_InvoiceId",
                principalTable: "ProductInvoice",
                principalColumn: "Id");
        }
    }
}
