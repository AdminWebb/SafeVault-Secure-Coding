using Xunit;
using BCrypt.Net;
using System.Text.Encodings.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SafeVault.Models;

namespace SafeVault.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void PasswordHashing_ShouldCreateValidHash()
        {
            // Arrange
            string plainText = "MySecurePass2026!";
            
            // Act
            string hash = BCrypt.Net.BCrypt.HashPassword(plainText);
            bool isValid = BCrypt.Net.BCrypt.Verify(plainText, hash);
            bool isInvalid = BCrypt.Net.BCrypt.Verify("WrongPassword", hash);

            // Assert
            Assert.True(isValid);
            Assert.False(isInvalid);
        }

        [Fact]
        public void XssSanitization_ShouldEncodeHtmlTags()
        {
            // Arrange
            string maliciousInput = "<script>alert('hacked!');</script>";

            // Act
            string sanitized = HtmlEncoder.Default.Encode(maliciousInput);

            // Assert
            Assert.DoesNotContain("<script>", sanitized);
            Assert.Equal("&lt;script&gt;alert(&#x27;hacked!&#x27;);&lt;/script&gt;", sanitized);
        }

        [Fact]
        public void UserDto_ShouldFailValidation_OnMaliciousUsername()
        {
            // Arrange
            var user = new UserDto 
            { 
                Username = "admin' OR '1'='1", // SQLi string
                Email = "test@test.com", 
                Password = "ValidPassword123!" 
            };
            
            var validationContext = new ValidationContext(user);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid); // Validation should fail due to the regex rule
        }
    }
}