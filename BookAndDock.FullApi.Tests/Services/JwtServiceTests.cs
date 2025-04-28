using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;

using Backend.Models;
using Backend.Services;

namespace Backend.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;
        private readonly byte[] _keyBytes;

        public JwtServiceTests()
        {
            // 1) In‐memory configuration with a 256‐bit key for HS256
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key",      "01234567890123456789012345678901" },  // 32 chars = 256 bits
                { "Jwt:Issuer",   "test-issuer" },
                { "Jwt:Audience", "test-audience" }
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // 2) Service under test
            _jwtService = new JwtService(_config);

            // 3) Precompute key bytes for validation
            _keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        }

        [Fact]
        public void GenerateToken_WithRole_ProducesValidJwtAndClaims()
        {
            // Arrange
            var user = new User
            {
                Id = 123,
                Email = "admin@example.com",
                Role = new Role { Id = 9, Name = "Admin" }
            };

            // Act
            var tokenString = _jwtService.GenerateToken(user);

            // Assert: token parses and validates
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateLifetime = false // no exp set in implementation
            };

            var principal = handler.ValidateToken(
                tokenString,
                validationParameters,
                out SecurityToken validatedToken
            );

            // Confirm it's a JWT
            Assert.IsType<JwtSecurityToken>(validatedToken);

            // Check the standard claims
            Assert.Equal(user.Id.ToString(),
                         principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(user.Email,
                         principal.FindFirst(ClaimTypes.Email)?.Value);
            Assert.Equal(user.Role.Name,
                         principal.FindFirst(ClaimTypes.Role)?.Value);
        }

        [Fact]
        public void GenerateToken_WithoutRole_DefaultsToUserRoleClaim()
        {
            // Arrange: user.Role is null
            var user = new User
            {
                Id = 77,
                Email = "guest@example.com",
                Role = null
            };

            // Act
            var tokenString = _jwtService.GenerateToken(user);

            // Assert: signature & issuer/audience validation
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateLifetime = false
            };

            var principal = handler.ValidateToken(
                tokenString,
                validationParameters,
                out SecurityToken validatedToken
            );

            // Should see “User” as the role if user.Role was null
            Assert.Equal("User",
                         principal.FindFirst(ClaimTypes.Role)?.Value);
        }
    }
}
