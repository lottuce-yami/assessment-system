using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssessmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AnswerOptionFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOption_Answer_AnswerId",
                table: "AnswerOption");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOption_AnswerId",
                table: "AnswerOption");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "AnswerOption");

            migrationBuilder.CreateTable(
                name: "AnswerAnswerOption",
                columns: table => new
                {
                    AnswersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SelectedOptionsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerAnswerOption", x => new { x.AnswersId, x.SelectedOptionsId });
                    table.ForeignKey(
                        name: "FK_AnswerAnswerOption_AnswerOption_SelectedOptionsId",
                        column: x => x.SelectedOptionsId,
                        principalTable: "AnswerOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerAnswerOption_Answer_AnswersId",
                        column: x => x.AnswersId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerAnswerOption_SelectedOptionsId",
                table: "AnswerAnswerOption",
                column: "SelectedOptionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerAnswerOption");

            migrationBuilder.AddColumn<Guid>(
                name: "AnswerId",
                table: "AnswerOption",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOption_AnswerId",
                table: "AnswerOption",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOption_Answer_AnswerId",
                table: "AnswerOption",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id");
        }
    }
}
