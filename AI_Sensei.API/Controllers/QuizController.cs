/* * QuizController.cs
 * * API Endpoints & Security
 * ------------------------
 * This controller manages the HTTP interface for the application.
 * It enforces security by validating JWT tokens and extracting the User ID
 * from the token claims to ensure users only access their own data.
 */

using Microsoft.AspNetCore.Mvc;
using AI_Sensei.Core;
using AI_Sensei.API.Services;
using AI_Sensei.API.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AI_Sensei.API.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateQuizRequest request)
        {

            var jsonQuiz = await _quizService.GenerateQuiz(
                request.Language, request.Topic, request.Difficulty
            );

            if (string.IsNullOrEmpty(jsonQuiz))
                return StatusCode(500, "AI returned empty quiz");

            var quiz = JsonSerializer.Deserialize<Quiz>(
                jsonQuiz,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return Ok(quiz);
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveQuiz([FromBody] Quiz quiz)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }

            quiz.UserId = userId;

            await _quizService.SaveQuiz(quiz);
            return Ok("Quiz saved successfully");
        }

        // Return a single quiz 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }

            var quiz = await _quizService.GetQuizById(userId, id);
            
            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }

            var quizzes = await _quizService.GetAll(userId);

            return Ok(quizzes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }

            var rowsDeleted = await _quizService.DeleteQuiz(userId, id);
            if (rowsDeleted == 0)
                return NotFound("Quiz not found");

            return NoContent();

        }
    }     
}
