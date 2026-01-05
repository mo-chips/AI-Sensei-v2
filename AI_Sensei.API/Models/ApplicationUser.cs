/* * ApplicationUser.cs
 * * Custom User Identity
 * --------------------
 * This class extends the default IdentityUser provided by ASP.NET Core.
 * It serves as the placeholder for any custom user profile data you might
 * want to add later (e.g., FullName, PremiumStatus, LearningGoals).
 */

using Microsoft.AspNetCore.Identity;

namespace AI_Sensei.API.Models
{
    public class ApplicationUser : IdentityUser
    {
    }
}
