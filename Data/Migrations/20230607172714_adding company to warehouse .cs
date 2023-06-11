using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    public partial class addingcompanytowarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyCode",
                table: "Warehouses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegisteredCompanyCompanyCode",
                table: "Warehouses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_RegisteredCompanyCompanyCode",
                table: "Warehouses",
                column: "RegisteredCompanyCompanyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_RegisteredCompanies_RegisteredCompanyCompanyCode",
                table: "Warehouses",
                column: "RegisteredCompanyCompanyCode",
                principalTable: "RegisteredCompanies",
                principalColumn: "CompanyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_RegisteredCompanies_RegisteredCompanyCompanyCode",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_RegisteredCompanyCompanyCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RegisteredCompanyCompanyCode",
                table: "Warehouses");
        }
    }
}
