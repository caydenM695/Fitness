using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    distanceRan = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    time = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    weightBenched = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    weightDeadlift = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    weightCurl = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises");
        }
    }
}
