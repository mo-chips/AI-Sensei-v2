using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Sensei.API.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Topic = table.Column<string>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    Options = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
