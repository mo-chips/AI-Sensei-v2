/* * QuizService.cs
 * * Service Layer / Business Logic
 * ------------------------------
 * This class acts as the intermediary between the API Controller and the data sources.
 * It coordinates the flow of information by calling the AI generator (IQuizGenerator) 
 * to create content and the database repository (IQuizRepository) to save/retrieve it.
 * It ensures the Controller remains lightweight and focused only on HTTP requests.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AI_Sensei.Core
{

    public class QuizService
    {
        private readonly IQuizGenerator _quizGenerator;
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizGenerator quizGenerator, IQuizRepository quizRepository)
        {
            _quizGenerator = quizGenerator;
            _quizRepository = quizRepository;
        }

        public async Task<String> GenerateQuiz(string language, string topic, int difficulty)
        {
            var quizObj = await _quizGenerator.GenerateQuiz(language, topic, difficulty);

            return quizObj;
        }


        public async Task SaveQuiz(Quiz quiz)
        {
            await _quizRepository.SaveQuiz(quiz);
        }

        public async Task<int> DeleteQuiz(string userId, int Id) 
        {
            int deleted = await _quizRepository.DeleteQuiz(userId, Id);
            return deleted;
        }

        public async Task<Quiz?> GetQuizById(string userId, int quizId) 
        {
            var quiz = await _quizRepository.GetQuizById(userId, quizId);
            return quiz;
        }

        public async Task<List<Quiz>> GetAll(string user) {

           var quizzes =  await _quizRepository.GetAllQuizzes(user);
           return quizzes;
        }

    }
       
}
