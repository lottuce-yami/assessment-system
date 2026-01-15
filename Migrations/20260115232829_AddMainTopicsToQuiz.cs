using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssessmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMainTopicsToQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainTopics",
                table: "Quiz",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainTopics",
                table: "Quiz");
        }
    }
}
