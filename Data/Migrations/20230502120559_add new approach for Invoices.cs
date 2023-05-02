using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addnewapproachforInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Invoice_Client_Name",
                table: "UtilityInvoices",
                newName: "Invoice_Store_Name");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Invoices",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "TaxTotal",
                table: "Invoices",
                newName: "Tax");

            migrationBuilder.RenameColumn(
                name: "TaxPercentage",
                table: "Invoices",
                newName: "Subtotal");

            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Invoices",
                newName: "Invoice_Type");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Invoices",
                newName: "Invoice_Date");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Invoices",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                newName: "IX_Invoices_CompanyId");

            migrationBuilder.AddColumn<string>(
                name: "Invoice_Number",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Store_Tax_Number",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "customer_number",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Companies_CompanyId",
                table: "Invoices",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Companies_CompanyId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Invoice_Number",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Store_Tax_Number",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "customer_number",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Invoice_Store_Name",
                table: "UtilityInvoices",
                newName: "Invoice_Client_Name");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Invoices",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "Tax",
                table: "Invoices",
                newName: "TaxTotal");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "Invoices",
                newName: "TaxPercentage");

            migrationBuilder.RenameColumn(
                name: "Invoice_Type",
                table: "Invoices",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "Invoice_Date",
                table: "Invoices",
                newName: "InvoiceDate");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Invoices",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CompanyId",
                table: "Invoices",
                newName: "IX_Invoices_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Invoices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "ExchangeRate",
                table: "Invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Invoices",
                type: "datetime2",
                maxLength: 50,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                table: "Invoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
