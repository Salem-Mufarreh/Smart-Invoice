using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addedCustomerNormilizationv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company_Id",
                table: "UtilityInvoices");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "UtilityInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UtilityInvoices_CustomerId",
                table: "UtilityInvoices",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UtilityInvoices_Customers_CustomerId",
                table: "UtilityInvoices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtilityInvoices_Customers_CustomerId",
                table: "UtilityInvoices");

            migrationBuilder.DropIndex(
                name: "IX_UtilityInvoices_CustomerId",
                table: "UtilityInvoices");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "UtilityInvoices");

            migrationBuilder.AddColumn<string>(
                name: "Company_Id",
                table: "UtilityInvoices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
