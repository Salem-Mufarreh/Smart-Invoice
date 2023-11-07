using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class switchingproductsinsalesaddingnewmodel : Migration
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

            migrationBuilder.CreateTable(
                name: "GinvoiceProp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    SalesInvoiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GinvoiceProp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GinvoiceProp_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_GinvoiceProp_salesInvoices_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "salesInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GinvoiceProp_productId",
                table: "GinvoiceProp",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_GinvoiceProp_SalesInvoiceId",
                table: "GinvoiceProp",
                column: "SalesInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GinvoiceProp");

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
