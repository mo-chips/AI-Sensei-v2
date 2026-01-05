/* * QuizRepository.cs
 * * Database Implementation
 * -----------------------
 * This class performs the actual SQL operations using Entity Framework Core.
 * It strictly enforces data ownership by including the 'userId' in every WHERE clause.
 */

using AI_Sensei.API.Data;
using AI_Sensei.Core;
using Microsoft.EntityFrameworkCore;


namespace AI_Sensei.API.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _dbContext;
        public QuizRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task SaveQuiz(Quiz quiz)
        {
            _dbContext.Quizzes.Add(quiz);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Quiz>> GetAllQuizzes(string userId)
        {
            return await _dbContext.Quizzes
                .Include(q => q.Questions)
                .Where(q => q.UserId == userId)
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizById(string userId, int quizId)
        {
            return await _dbContext.Quizzes
                .Include(q => q.Questions)
                .Where(q => q.UserId == userId && q.Id == quizId)
                .FirstOrDefaultAsync();
                
        }

        public async Task<int> DeleteQuiz(string userId, int quizId)
        {
            return await _dbContext.Quizzes
                .Where(q => q.UserId == userId && q.Id == quizId)
                .ExecuteDeleteAsync();
        }
    }
}
