/* * IQuizRepository.cs
 * * Data Access Interface
 * ---------------------
 * This interface defines the contract for data persistence.
 * It ensures that the application stays decoupled from the specific database technology.
 * The QuizService relies on these methods to perform CRUD (Create, Read, Delete) operations.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AI_Sensei.Core
{
    public interface IQuizRepository
    {
        Task SaveQuiz(Quiz quiz);
        Task<List<Quiz>> GetAllQuizzes(string userId);
        Task<Quiz?> GetQuizById(string userId, int quizId);
        Task<int> DeleteQuiz(string userId, int quizID);
    }
}
