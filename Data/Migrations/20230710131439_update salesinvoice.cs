using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class updatesalesinvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_salesInvoices_SalesInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_SalesInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.AddColumn<int>(
                name: "SalesInvoiceId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SalesInvoiceId",
                table: "Products",
                column: "SalesInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_salesInvoices_SalesInvoiceId",
                table: "Products",
                column: "SalesInvoiceId",
                principalTable: "salesInvoices",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_salesInvoices_SalesInvoiceId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SalesInvoiceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "SalesInvoiceId",
                table: "InvoiceItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_SalesInvoiceId",
                table: "InvoiceItem",
                column: "SalesInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_salesInvoices_SalesInvoiceId",
                table: "InvoiceItem",
                column: "SalesInvoiceId",
                principalTable: "salesInvoices",
                principalColumn: "Id");
        }
    }
}
