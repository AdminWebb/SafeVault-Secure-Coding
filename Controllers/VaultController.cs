using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Text.Encodings.Web;

namespace SafeVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaultController : ControllerBase
    {
        private readonly string _connectionString = "Server=myServer;Database=SafeVaultDB;User Id=myUser;Password=myPassword;";

        // RBAC: Only Admins can access this endpoint
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-data")]
        public IActionResult GetAdminData()
        {
            return Ok("Confidential Admin Vault Data Accessed.");
        }

        // SQL Injection Prevention & XSS Prevention
        [HttpGet("search")]
        public IActionResult SearchVault([FromQuery] string searchTerm)
        {
            // 1. XSS Prevention: Sanitize the input before echoing it back to the user/DOM
            string sanitizedSearch = HtmlEncoder.Default.Encode(searchTerm);

            // 2. SQL Injection Prevention: Using Parameterized Queries
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT VaultItem FROM Vaults WHERE ItemName = @SearchTerm";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parameter explicitly bound, preventing SQL injection
                    command.Parameters.AddWithValue("@SearchTerm", sanitizedSearch);
                    
                    // connection.Open();
                    // using (SqlDataReader reader = command.ExecuteReader()) { ... }
                }
            }

            return Ok(new { Message = $"Searched vault securely for: {sanitizedSearch}" });
        }
    }
}