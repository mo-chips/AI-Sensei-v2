/* * OpenAIQuizService.cs
 * * AI Integration Implementation
 * -----------------------------
 * This service formats the user's request into a strict prompt for OpenAI.
 * It enforces a specific JSON structure so the C# app can parse the result.
 * * SECURITY NOTE: The 'ChatClient' is injected via the constructor. 
 * This ensures the API Key is not hardcoded here, but loaded from configuration.
 */

using OpenAI;

using AI_Sensei.Core;
using OpenAI.Chat;

namespace AI_Sensei.API.Services
{
    public class OpenAIQuizService : IQuizGenerator
    {
        private readonly ChatClient _chatClient;
        public OpenAIQuizService(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<String> GenerateQuiz(string language, string topic, int difficulty)
        {
            string jsonFormat = @"
            {
                ""Title"": ""Quiz Title Here"",
                ""Topic"": ""Quiz Topic"",
                ""Difficulty"": 1-5, 
                ""Questions"": [
                    {
                        ""QuestionNumber"": 1,
                        ""QuestionText"": ""What is...?"",
                        ""Options"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
                        ""CorrectAnswer"": ""Option C"",
                        ""UserAnswer"": """",
                        ""IsCorrect"": false
                    }
                ]
            }";

            string prompt = $@"
            Generate a {language} quiz about {topic} with a difficulty of {difficulty} out of 5.
            The quiz must contain exactly 5 theory questions.
            
            
            IMPORTANT RULES:
            - Respond ONLY with a valid JSON object.
            - Do NOT include any markdown formatting.
            - Do NOT include code fences such as ```json.
            - Do NOT include any explanation or text outside the JSON.
            - All JSON property names MUST use PascalCase to match C# classes.
            - Follow this exact JSON structure and field names:
            {jsonFormat}

            ";

            Console.WriteLine("\nGenerating JSON quiz... please wait.");

            try
            {
                ChatCompletion response = await _chatClient.CompleteChatAsync(prompt);
                string outputText = response.Content[0].Text;
                Console.WriteLine("\n--- AI Response (Raw JSON) ---");
                Console.WriteLine(outputText);

                return outputText;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"API Error: {ex.Message}");
               
                return null;
            }
        }
    }

    
  
}
