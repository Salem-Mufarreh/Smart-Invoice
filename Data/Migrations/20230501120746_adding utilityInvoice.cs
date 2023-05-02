using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addingutilityInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UtilityInvoices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Invoice_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_Client_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Incoive_Companay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company_License_Registration_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Invoice_Amount = table.Column<double>(type: "float", nullable: true),
                    Invoice_VAT = table.Column<double>(type: "float", nullable: true),
                    Service_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Meter_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Previous_Reading_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Current_Reading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Previous_Debt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityInvoices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilityInvoices");
        }
    }
}
