using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Text.Json;
using Backend.Data;

namespace Backend.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly BookAndDockContext _context;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();

            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new BookAndDockContext(options);

            _controller = new UsersController(_context, _userServiceMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOk()
        {
            _context.Users.Add(new User
            {
                Id = 1,
                Name = "Test",
                Surname = "User",
                Email = "test@example.com",
                PhoneNumber = "123456",
                Password = "hashedpassword",
                Role = new Role { Id = 1, Name = "Admin" }
            });
            await _context.SaveChangesAsync();

            var result = await _controller.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
            Assert.Single(users);
        }

        [Fact]
        public async Task DeleteUser_WhenUserExists_ReturnsOk()
        {
            var user = new User
            {
                Id = 2,
                Name = "Del",
                Surname = "Me",
                Email = "del@example.com",
                Password = "hashedpassword"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteUser(2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            Assert.Contains("User deleted", json);
        }

        [Fact]
        public void GetUserCountsByRoles_ReturnsCounts()
        {
            var expected = new Dictionary<string, int> { { "User", 2 }, { "Admin", 1 } };
            _userServiceMock.Setup(s => s.CountUsersByRoles()).Returns(expected);

            var result = _controller.GetUserCountsByRoles();

            Assert.Equal(expected, result);
        }
    }
}
