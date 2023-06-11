using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class RegisteredCompanies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyCode",
                table: "Invoices",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegisteredCompanyCompanyCode",
                table: "Invoices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegisteredCompanies",
                columns: table => new
                {
                    CompanyCode = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GovCompanyRegistration = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCompanies", x => x.CompanyCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RegisteredCompanyCompanyCode",
                table: "Invoices",
                column: "RegisteredCompanyCompanyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_RegisteredCompanies_RegisteredCompanyCompanyCode",
                table: "Invoices",
                column: "RegisteredCompanyCompanyCode",
                principalTable: "RegisteredCompanies",
                principalColumn: "CompanyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_RegisteredCompanies_RegisteredCompanyCompanyCode",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "RegisteredCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_RegisteredCompanyCompanyCode",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RegisteredCompanyCompanyCode",
                table: "Invoices");
        }
    }
}
