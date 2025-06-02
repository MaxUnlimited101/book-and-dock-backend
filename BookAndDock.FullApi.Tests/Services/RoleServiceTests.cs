using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _roleRepoMock = new Mock<IRoleRepository>();
            _roleService = new RoleService(_roleRepoMock.Object);
        }

        [Fact]
        public async Task GetAllRolesAsync_ReturnsRoles()
        {
            // Arrange
            var roles = new List<RoleDTO>
            {
                new RoleDTO(1, "Admin", DateTime.UtcNow),
                new RoleDTO(2, "User", DateTime.UtcNow)
            };
            _roleRepoMock.Setup(repo => repo.GetAllRolesAsync()).ReturnsAsync(roles);

            // Act
            var result = await _roleService.GetAllRolesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Name == "Admin");
        }

        [Fact]
        public async Task GetRoleByIdAsync_Exists_ReturnsRole()
        {
            // Arrange
            var role = new RoleDTO(1, "Admin", DateTime.UtcNow);
            _roleRepoMock.Setup(repo => repo.GetRoleByIdAsync(1)).ReturnsAsync(role);

            // Act
            var result = await _roleService.GetRoleByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result!.Name);
        }

        [Fact]
        public async Task GetRoleByIdAsync_NotExists_ReturnsNull()
        {
            // Arrange
            _roleRepoMock.Setup(repo => repo.GetRoleByIdAsync(999)).ReturnsAsync((RoleDTO?)null);

            // Act
            var result = await _roleService.GetRoleByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateRoleAsync_Valid_ReturnsNewId()
        {
            // Arrange
            var newRole = new RoleDTO(0, "Editor", DateTime.UtcNow);
            _roleRepoMock.Setup(repo => repo.CreateRoleAsync(newRole)).ReturnsAsync(123);

            // Act
            var result = await _roleService.CreateRoleAsync(newRole);

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public async Task UpdateRoleAsync_Valid_ReturnsTrue()
        {
            // Arrange
            var updatedRole = new RoleDTO(1, "UpdatedName", DateTime.UtcNow);
            _roleRepoMock.Setup(repo => repo.UpdateRoleAsync(updatedRole)).ReturnsAsync(true);

            // Act
            var result = await _roleService.UpdateRoleAsync(updatedRole);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateRoleAsync_Fails_ReturnsFalse()
        {
            // Arrange
            var updatedRole = new RoleDTO(99, "NonExistent", DateTime.UtcNow);
            _roleRepoMock.Setup(repo => repo.UpdateRoleAsync(updatedRole)).ReturnsAsync(false);

            // Act
            var result = await _roleService.UpdateRoleAsync(updatedRole);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteRoleAsync_CallsRepository()
        {
            // Arrange
            int roleId = 5;

            // Act
            await _roleService.DeleteRoleAsync(roleId);

            // Assert
            _roleRepoMock.Verify(r => r.DeleteRoleAsync(roleId), Times.Once);
        }
    }
}
