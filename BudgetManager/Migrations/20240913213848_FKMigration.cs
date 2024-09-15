using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetManager.Migrations
{
    /// <inheritdoc />
    public partial class FKMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthPatterns_Patterns_PatternId",
                table: "MonthPatterns");

            migrationBuilder.AlterColumn<int>(
                name: "PatternId",
                table: "MonthPatterns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthPatterns_Patterns_PatternId",
                table: "MonthPatterns",
                column: "PatternId",
                principalTable: "Patterns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthPatterns_Patterns_PatternId",
                table: "MonthPatterns");

            migrationBuilder.AlterColumn<int>(
                name: "PatternId",
                table: "MonthPatterns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthPatterns_Patterns_PatternId",
                table: "MonthPatterns",
                column: "PatternId",
                principalTable: "Patterns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
