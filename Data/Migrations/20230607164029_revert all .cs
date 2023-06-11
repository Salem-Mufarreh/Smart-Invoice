using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class revertall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisteredCompanies",
                columns: table => new
                {
                    RegisteredCompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCompanies", x => x.RegisteredCompanyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCompanies_CompanyCode",
                table: "RegisteredCompanies",
                column: "CompanyCode",
                unique: true,
                filter: "[CompanyCode] IS NOT NULL");
        }
    }
}
