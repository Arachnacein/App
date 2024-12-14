using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetManager.Migrations
{
    /// <inheritdoc />
    public partial class RecreateMonthPatternTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthPatterns");

            migrationBuilder.CreateTable(
                name: "MonthPatterns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatternId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthPatterns_Patterns_PatternId",
                        column: x => x.PatternId,
                        principalTable: "Patterns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthPatterns_PatternId",
                table: "MonthPatterns",
                column: "PatternId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "MonthPatterns");
        }
    }
}