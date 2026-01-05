/* * GenerateQuizRequest.cs
 * * API Data Transfer Object
 * ------------------------
 * This model defines the payload expected by the "Generate Quiz" endpoint.
 * It uses Data Annotations ([Required], [Range]) to automatically validate 
 * inputs, ensuring the AI service doesn't receive invalid parameters.
 */

using System.ComponentModel.DataAnnotations;

namespace AI_Sensei.API.Models
{
    public class GenerateQuizRequest
    {
        [Required]
        public string Language { get; set; }

        [Required]
        public string Topic { get; set; }

        [Range(1,5)]
        public int Difficulty { get; set; }
    }
}
