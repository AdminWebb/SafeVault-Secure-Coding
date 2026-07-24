using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using SafeVault.Models;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Mock database for demonstration
        private static string _storedHash = BCrypt.Net.BCrypt.HashPassword("SuperSecretPassword123!");

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input format.");
            }

            // Secure Authentication: Verifying hash instead of plain text
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, _storedHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            // (In a real app, JWT generation would happen here)
            return Ok(new { Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", Role = "Admin" });
        }
    }
}