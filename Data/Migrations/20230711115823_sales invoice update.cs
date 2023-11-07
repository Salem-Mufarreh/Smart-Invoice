using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class salesinvoiceupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GinvoiceProp_Products_productId",
                table: "GinvoiceProp");

            migrationBuilder.DropForeignKey(
                name: "FK_GinvoiceProp_salesInvoices_SalesInvoiceId",
                table: "GinvoiceProp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GinvoiceProp",
                table: "GinvoiceProp");

            migrationBuilder.RenameTable(
                name: "GinvoiceProp",
                newName: "ginvoices");

            migrationBuilder.RenameIndex(
                name: "IX_GinvoiceProp_SalesInvoiceId",
                table: "ginvoices",
                newName: "IX_ginvoices_SalesInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_GinvoiceProp_productId",
                table: "ginvoices",
                newName: "IX_ginvoices_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ginvoices",
                table: "ginvoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ginvoices_Products_productId",
                table: "ginvoices",
                column: "productId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ginvoices_salesInvoices_SalesInvoiceId",
                table: "ginvoices",
                column: "SalesInvoiceId",
                principalTable: "salesInvoices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ginvoices_Products_productId",
                table: "ginvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ginvoices_salesInvoices_SalesInvoiceId",
                table: "ginvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ginvoices",
                table: "ginvoices");

            migrationBuilder.RenameTable(
                name: "ginvoices",
                newName: "GinvoiceProp");

            migrationBuilder.RenameIndex(
                name: "IX_ginvoices_SalesInvoiceId",
                table: "GinvoiceProp",
                newName: "IX_GinvoiceProp_SalesInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ginvoices_productId",
                table: "GinvoiceProp",
                newName: "IX_GinvoiceProp_productId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GinvoiceProp",
                table: "GinvoiceProp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GinvoiceProp_Products_productId",
                table: "GinvoiceProp",
                column: "productId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_GinvoiceProp_salesInvoices_SalesInvoiceId",
                table: "GinvoiceProp",
                column: "SalesInvoiceId",
                principalTable: "salesInvoices",
                principalColumn: "Id");
        }
    }
}
