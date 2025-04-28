
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

using Backend.Data;
using Backend.Services;
using Backend.Models;
using Backend.DTO;

namespace Backend.Tests.Services
{
    public class RegistrationServiceTests : IDisposable
    {
        private readonly BookAndDockContext _context;
        private readonly RegistrationService _svc;

        public RegistrationServiceTests()
        {
            // 1) In-memory EF Core context
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new BookAndDockContext(options);

            // 2) Service under test
            _svc = new RegistrationService(_context);
        }

        [Fact]
        public async Task RegisterAsync_NewEmail_CreatesUser()
        {
            // Arrange
            var dto = new RegisterRequestDto
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                Password = "Passw0rd!"
            };

            // Act
            var result = await _svc.RegisterAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0, "Expected database to assign an Id");
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Surname, result.Surname);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(1, result.RoleId);
            Assert.NotNull(result.CreatedOn);

            // Password should be stored hashed
            Assert.True(BCrypt.Net.BCrypt.Verify(dto.Password, result.Password));

            // And the user should actually be in the context
            var persisted = await _context.Users.FindAsync(result.Id);
            Assert.NotNull(persisted);
            Assert.Equal(dto.Email, persisted.Email);
        }

        [Fact]
        public async Task RegisterAsync_DuplicateEmail_ThrowsException()
        {
            // Arrange: seed a user with this email
            var existing = new User
            {
                Name = "Jane",
                Surname = "Smith",
                Email = "jane.smith@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Initial1!"),
                RoleId = 1,
                CreatedOn = DateTime.UtcNow
            };
            _context.Users.Add(existing);
            await _context.SaveChangesAsync();

            var dto = new RegisterRequestDto
            {
                Name = "Jane",
                Surname = "Smith",
                Email = "jane.smith@example.com",
                Password = "AnotherPass1!"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _svc.RegisterAsync(dto));
            Assert.Equal("Email is already registered.", ex.Message);
        }

        public void Dispose() => _context.Dispose();
    }
}
