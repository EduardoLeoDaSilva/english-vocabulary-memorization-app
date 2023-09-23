using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVocabularyMemorization.Migrations
{
    /// <inheritdoc />
    public partial class examentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastExam",
                table: "Words",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OriginalSentence",
                table: "Sentences",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsFinished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastExam = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExamSentence",
                columns: table => new
                {
                    ExamsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SentencesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSentence", x => new { x.ExamsId, x.SentencesId });
                    table.ForeignKey(
                        name: "FK_ExamSentence_Exams_ExamsId",
                        column: x => x.ExamsId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSentence_Sentences_SentencesId",
                        column: x => x.SentencesId,
                        principalTable: "Sentences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSentence_SentencesId",
                table: "ExamSentence",
                column: "SentencesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSentence");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropColumn(
                name: "LastExam",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "OriginalSentence",
                table: "Sentences");
        }
    }
}
