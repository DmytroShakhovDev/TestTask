using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestProject.WebAPI.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(nullable: false),
                    Credit = table.Column<decimal>(nullable: false)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    MonthlySalary = table.Column<decimal>(nullable: false),
                    MonthlyExpenses = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
