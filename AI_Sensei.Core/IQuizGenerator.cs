/* * IQuizGenerator.cs
 * * AI Integration Interface
 * ------------------------
 * This contract defines how the application requests new content from an AI service.
 * It returns the raw JSON string from the AI, which the Service layer then handles.
 * This abstraction allows you to switch between OpenAI, Gemini, or Claude easily.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Sensei.Core
{
    public interface IQuizGenerator
    {
        Task<String> GenerateQuiz(string language, string topic, int difficulty);
    }
}
