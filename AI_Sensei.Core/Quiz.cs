/* * Quiz.cs - Domain Models
 * -----------------------
 * These classes define the core data structure of the application.
 * 'Quiz' acts as the parent entity containing metadata (Title, Difficulty),
 * while 'QuizQuestion' represents individual items linked to that quiz.
 * These models are used for both database storage (SQLite) and API responses.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AI_Sensei.Core
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public string CorrectAnswer { get; set; } = string.Empty;

        public string UserAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public int QuizId { get; set; }
    }

    public class Quiz
    {
        public string UserId { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public int Difficulty { get; set; }
        public List<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

    }
}
