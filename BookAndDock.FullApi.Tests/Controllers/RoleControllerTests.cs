using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _roleServiceMock;
        private readonly RoleController _controller;

        public RoleControllerTests()
        {
            _roleServiceMock = new Mock<IRoleService>();
            _controller = new RoleController(_roleServiceMock.Object);
        }

        [Fact]
        public async Task GetAllRoles_ReturnsList()
        {
            var roles = new List<RoleDTO>
            {
                new RoleDTO(1, "Admin", DateTime.UtcNow)
            };
            _roleServiceMock.Setup(s => s.GetAllRolesAsync()).ReturnsAsync(roles);

            var result = await _controller.GetAllRoles();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<RoleDTO>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task GetRoleById_WhenExists_ReturnsOk()
        {
            var role = new RoleDTO(1, "Admin", DateTime.UtcNow);
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);

            var result = await _controller.GetRoleById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<RoleDTO>(okResult.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public async Task GetRoleById_WhenNotFound_ReturnsNotFound()
        {
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((RoleDTO?)null);

            var result = await _controller.GetRoleById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateRole_Valid_ReturnsCreated()
        {
            var dto = new RoleDTO(0, "Admin", DateTime.UtcNow);
            _roleServiceMock.Setup(s => s.CreateRoleAsync(dto)).ReturnsAsync(1);

            var result = await _controller.CreateRole(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(1, created.Value);
        }

        [Fact]
        public async Task CreateRole_Null_ReturnsBadRequest()
        {
            var result = await _controller.CreateRole(null);

            var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Role data is required.", bad.Value);
        }

        [Fact]
        public async Task UpdateRole_Valid_ReturnsNoContent()
        {
            var dto = new RoleDTO(1, "Admin", DateTime.UtcNow);
            _roleServiceMock.Setup(s => s.UpdateRoleAsync(dto)).ReturnsAsync(true);

            var result = await _controller.UpdateRole(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateRole_InvalidId_ReturnsBadRequest()
        {
            var dto = new RoleDTO(2, "Admin", DateTime.UtcNow);

            var result = await _controller.UpdateRole(1, dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Role data is invalid.", bad.Value);
        }

        [Fact]
        public async Task UpdateRole_NotFound_ReturnsNotFound()
        {
            var dto = new RoleDTO(1, "Admin", DateTime.UtcNow);
            _roleServiceMock.Setup(s => s.UpdateRoleAsync(dto)).ReturnsAsync(false);

            var result = await _controller.UpdateRole(1, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteRole_Exists_ReturnsNoContent()
        {
            var dto = new RoleDTO(1, "Admin", DateTime.UtcNow);
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(dto);

            var result = await _controller.DeleteRole(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteRole_NotFound_ReturnsNotFound()
        {
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((RoleDTO?)null);

            var result = await _controller.DeleteRole(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
