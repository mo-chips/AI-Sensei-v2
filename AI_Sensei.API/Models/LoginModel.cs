/* * LoginModel.cs
 * * Authentication DTO
 * --------------------
 * This simple object captures the raw user input from the login/register forms.
 * It is automatically validated by ASP.NET Core before the controller sees it.
 */

using System.ComponentModel.DataAnnotations;

namespace AI_Sensei.API.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required] 
        public string Password { get; set;  }
    }
}
