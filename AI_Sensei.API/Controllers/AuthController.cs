/* * AuthController.cs
 * * User Authentication & JWT Generation
 * ------------------------------------
 * This controller manages user identities. It uses ASP.NET Core Identity
 * to validate credentials and generates a secure JSON Web Token (JWT)
 * for the client to use in future authorized requests.
 */

using AI_Sensei.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AI_Sensei.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            // Find the user
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                model.Password,
                lockoutOnFailure: true
                );

            if (result.IsLockedOut)
                return StatusCode(423, "Account is locked out. Try again later.");

            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");


            // Generate the Token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
