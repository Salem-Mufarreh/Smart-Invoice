using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class DropRegisteredCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredCompanies");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
