using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class updateinvoicenumbertostring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Invoice_number",
                table: "salesInvoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Invoice_number",
                table: "salesInvoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
