// File: Backend.Tests/Services/UserServiceTests.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly UserService _svc;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _svc = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public void CheckIfUserExistsByIdAndRole_CallsRepository()
        {
            // Arrange
            _userRepoMock.Setup(r => r.CheckIfUserExistsByIdAndRole(1, It.IsAny<Role>())).Returns(true);

            // Act
            var result = _svc.CheckIfUserExistsByIdAndRole(1, new Role());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllUsersByIdAsync_ReturnsList()
        {
            // Arrange
            var users = new List<User> { new User { Id = 1 }, new User { Id = 2 } };
            _userRepoMock.Setup(r => r.GetAllUsersByIdAsync()).ReturnsAsync(users);

            // Act
            var result = await _svc.GetAllUsersByIdAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 3 };
            _userRepoMock.Setup(r => r.GetUserByIdAsync(3)).ReturnsAsync(user);

            // Act
            var result = await _svc.GetUserByIdAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 4, Email = "test@example.com" };
            _userRepoMock.Setup(r => r.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act
            var result = await _svc.GetUserByEmailAsync(user.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result!.Email);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 5 };
            _userRepoMock.Setup(r => r.GetUserByUsernameAsync("username")).ReturnsAsync(user);

            // Act
            var result = await _svc.GetUserByUsernameAsync("username");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
        }

        [Fact]
        public async Task GetUserByPhoneNumberAsync_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 6 };
            _userRepoMock.Setup(r => r.GetUserByPhoneNumberAsync("123456789")).ReturnsAsync(user);

            // Act
            var result = await _svc.GetUserByPhoneNumberAsync("123456789");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result!.Id);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_CallsRepository()
        {
            // Arrange
            _userRepoMock.Setup(r => r.UpdateUserById(1, It.IsAny<User>())).Returns(true);

            // Act
            var result = await _svc.UpdateUserByIdAsync(1, new User());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_CallsRepository()
        {
            // Arrange
            _userRepoMock.Setup(r => r.DeleteUserAsync(2)).Returns(Task.CompletedTask);

            // Act
            await _svc.DeleteUserByIdAsync(2);

            // Assert
            _userRepoMock.Verify(r => r.DeleteUserAsync(2), Times.Once);
        }

        [Fact]
        public void CountUsersByRoles_ReturnsDictionary()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "Admin", 1 }, { "User", 5 } };
            _userRepoMock.Setup(r => r.CountUsersByRoles()).Returns(dict);

            // Act
            var result = _svc.CountUsersByRoles();

            // Assert
            Assert.Equal(dict, result);
        }
    }
}
