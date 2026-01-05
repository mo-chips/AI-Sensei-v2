/* * AppDbContext.cs
 * * Database Configuration
 * ---------------------
 * Configures the database connection.
 * 'OnModelCreating' is required for Identity (Users/Login) to work properly.
 */

using AI_Sensei.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AI_Sensei.Core;

namespace AI_Sensei.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
