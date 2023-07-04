using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class fixedwarehousecompanycode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegisteredCompanyCompanyCode",
                table: "Warehouses",
                newName: "RCompanyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RCompanyCode",
                table: "Warehouses",
                newName: "RegisteredCompanyCompanyCode");
        }
    }
}
