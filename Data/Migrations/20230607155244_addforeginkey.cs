using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addforeginkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegisteredCompanies_CompanyCode",
                table: "RegisteredCompanies");

            migrationBuilder.RenameColumn(
                name: "CompanyCode",
                table: "Invoices",
                newName: "RegisteredCompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "RegisteredCompanies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCompanies_CompanyCode",
                table: "RegisteredCompanies",
                column: "CompanyCode",
                unique: true,
                filter: "[CompanyCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RegisteredCompanies_CompanyCode",
                table: "RegisteredCompanies");

            migrationBuilder.RenameColumn(
                name: "RegisteredCompanyId",
                table: "Invoices",
                newName: "CompanyCode");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "RegisteredCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCompanies_CompanyCode",
                table: "RegisteredCompanies",
                column: "CompanyCode",
                unique: true);
        }
    }
}
