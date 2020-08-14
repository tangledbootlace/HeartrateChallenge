using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorHRC.Migrations
{
    public partial class Numba2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Zone1LowerBound = table.Column<int>(nullable: false),
                    Zone1UpperBound = table.Column<int>(nullable: false),
                    Zone2LowerBound = table.Column<int>(nullable: false),
                    Zone2UpperBound = table.Column<int>(nullable: false),
                    Zone3LowerBound = table.Column<int>(nullable: false),
                    Zone3UpperBound = table.Column<int>(nullable: false),
                    Zone4LowerBound = table.Column<int>(nullable: false),
                    Zone4UpperBound = table.Column<int>(nullable: false),
                    Zone5LowerBound = table.Column<int>(nullable: false),
                    Zone5UpperBound = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
