using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addedregistedcompanytoinventoryv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyCode",
                table: "Inventories",
                newName: "RegisteredCompanyCompanyCode");

            migrationBuilder.AddColumn<long>(
                name: "RCompanyCode",
                table: "Inventories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_RegisteredCompanyCompanyCode",
                table: "Inventories",
                column: "RegisteredCompanyCompanyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_RegisteredCompanies_RegisteredCompanyCompanyCode",
                table: "Inventories",
                column: "RegisteredCompanyCompanyCode",
                principalTable: "RegisteredCompanies",
                principalColumn: "CompanyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_RegisteredCompanies_RegisteredCompanyCompanyCode",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_RegisteredCompanyCompanyCode",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "RCompanyCode",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "RegisteredCompanyCompanyCode",
                table: "Inventories",
                newName: "CompanyCode");
        }
    }
}
