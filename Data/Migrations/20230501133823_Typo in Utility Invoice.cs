using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class TypoinUtilityInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Incoive_Companay",
                table: "UtilityInvoices",
                newName: "Incoive_Company");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Incoive_Company",
                table: "UtilityInvoices",
                newName: "Incoive_Companay");
        }
    }
}
