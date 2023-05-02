using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class UtilityInvoiceinheritsfromInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilityInvoices");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Current_Reading",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Invoice_Id",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Meter_Number",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Previous_Debt",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Previous_Reading_Date",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service_Number",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Current_Reading",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Invoice_Id",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Meter_Number",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Previous_Debt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Previous_Reading_Date",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Service_Number",
                table: "Invoices");

            migrationBuilder.CreateTable(
                name: "UtilityInvoices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company_License_Registration_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Current_Reading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Incoive_Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_Amount = table.Column<double>(type: "float", nullable: true),
                    Invoice_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_Store_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_VAT = table.Column<double>(type: "float", nullable: true),
                    Meter_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Previous_Debt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Previous_Reading_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service_Number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilityInvoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtilityInvoices_CompanyId",
                table: "UtilityInvoices",
                column: "CompanyId");
        }
    }
}
