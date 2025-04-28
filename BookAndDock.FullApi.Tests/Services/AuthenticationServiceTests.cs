using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

using Backend.Data;
using Backend.Services;
using Backend.Models;


namespace Backend.Tests.Services
{
    public class AuthenticationServiceTests : IDisposable
    {
        private readonly BookAndDockContext _context;
        private readonly JwtService _jwtService;
        private readonly AuthenticationService _svc;

        public AuthenticationServiceTests()
        {
            // 1) In-memory EF Core context
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new BookAndDockContext(options);

            // 2) In-memory configuration with a 256-bit (32-char) key
            var inMemorySettings = new Dictionary<string, string>
            {
                // Key must be at least 32 bytes (256 bits) for HS256
                { "Jwt:Key",     "01234567890123456789012345678901" },
                { "Jwt:Issuer",  "test-issuer" },
                { "Jwt:Audience","test-audience" }
            };
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // 3) Real JwtService and the system under test
            _jwtService = new JwtService(config);
            _svc = new AuthenticationService(_context, _jwtService);
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsTokenWithCorrectClaims()
        {
            // Arrange
            var plain = "Secret123!";
            var hash = BCrypt.Net.BCrypt.HashPassword(plain);
            var user = new User
            {
                Id = 42,
                Name = "Alice",    // required non-nullable properties
                Surname = "Smith",
                Email = "alice@example.com",
                Password = hash,
                Role = new Role { Id = 1, Name = "Admin" }
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new LoginRequestDto
            {
                Email = user.Email,
                Password = plain
            };

            // Act
            var tokenString = await _svc.AuthenticateAsync(dto);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenString);

            Assert.Equal(user.Id.ToString(),
                         jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(user.Email,
                         jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            Assert.Equal(user.Role.Name,
                         jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        }

        [Theory]
        [InlineData("bob@example.com", "Secret123!")]  // wrong email
        [InlineData("alice@example.com", "BadPass!")]    // wrong password
        public async Task AuthenticateAsync_InvalidCredentials_Throws(string email, string password)
        {
            // Arrange: seed only the valid user with all required fields
            var validHash = BCrypt.Net.BCrypt.HashPassword("Secret123!");
            _context.Users.Add(new User
            {
                Name = "Alice",
                Surname = "Smith",
                Email = "alice@example.com",
                Password = validHash,
                Role = new Role { Id = 1, Name = "User" }
            });
            await _context.SaveChangesAsync();

            var dto = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _svc.AuthenticateAsync(dto));
            Assert.Equal("Invalid email or password.", ex.Message);
        }

        public void Dispose() => _context.Dispose();
    }
}
