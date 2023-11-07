using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class salsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountPaid",
                table: "salesInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BalanceDue",
                table: "salesInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "salesInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "salesInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "salesInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Invoice_number",
                table: "salesInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "salesInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "salesInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SubTotal",
                table: "salesInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Tax",
                table: "salesInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "salesInvoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SalesInvoiceId",
                table: "InvoiceItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_salesInvoices_CustomerId",
                table: "salesInvoices",
                column: "CustomerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_salesInvoices_Customers_CustomerId",
                table: "salesInvoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_salesInvoices_SalesInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_salesInvoices_Customers_CustomerId",
                table: "salesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_salesInvoices_CustomerId",
                table: "salesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_SalesInvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "BalanceDue",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "Invoice_number",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "salesInvoices");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceId",
                table: "InvoiceItem");
        }
    }
}
