using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class DockingSpotControllerTests
    {
        private readonly Mock<IDockingSpotService> _dockServiceMock;
        private readonly DockingSpotController _controller;

        public DockingSpotControllerTests()
        {
            _dockServiceMock = new Mock<IDockingSpotService>();
            _controller = new DockingSpotController(_dockServiceMock.Object);
        }

        [Fact]
        public async Task GetAvailableDockingSpots_ReturnsOkWithMappedResults()
        {
            var docks = new List<DockingSpot> { new DockingSpot { Id = 1 }, new DockingSpot { Id = 2 } };
            _dockServiceMock.Setup(s => s.GetAvailableDockingSpotsAsync(null, null, null, null, null)).ReturnsAsync(docks);

            var result = await _controller.GetAvailableDockingSpots(null, null, null, null, null);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<List<DockingSpotReturnDto>>(ok.Value);
            Assert.Equal(2, returned.Count);
        }

        [Fact]
        public void CreateDockingSpot_Valid_ReturnsCreated()
        {
            var dto = new DockingSpotDto(0, "Spot A", "Desc", 1, 1, 100, 10, true, DateTime.UtcNow);

            var result = _controller.CreateDockingSpot(dto);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void CreateDockingSpot_Invalid_ReturnsBadRequest()
        {
            var dto = new DockingSpotDto(0, "Spot A", "Desc", 1, 1, 100, 10, true, DateTime.UtcNow);
            _dockServiceMock.Setup(s => s.CreateDockingSpot(dto)).Throws(new ModelInvalidException("Invalid"));

            var result = _controller.CreateDockingSpot(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid", bad.Value);
        }

        [Fact]
        public void UpdateDockingSpot_Valid_ReturnsOk()
        {
            var dto = new DockingSpotDto(1, "Spot B", "Updated", 1, 1, 200, 20, false, DateTime.UtcNow);

            var result = _controller.UpdateDockingSpot(dto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateDockingSpot_Invalid_ReturnsBadRequest()
        {
            var dto = new DockingSpotDto(1, "Spot B", "Updated", 1, 1, 200, 20, false, DateTime.UtcNow);
            _dockServiceMock.Setup(s => s.UpdateDockingSpot(dto)).Throws(new ModelInvalidException("Failed"));

            var result = _controller.UpdateDockingSpot(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed", bad.Value);
        }

        [Fact]
        public void DeleteDockingSpot_Valid_ReturnsOk()
        {
            var result = _controller.DeleteDockingSpot(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteDockingSpot_Invalid_ReturnsBadRequest()
        {
            _dockServiceMock.Setup(s => s.DeleteDockingSpot(2)).Throws(new ModelInvalidException("Delete failed"));

            var result = _controller.DeleteDockingSpot(2);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Delete failed", bad.Value);
        }

        [Fact]
        public void GetDockingSpot_Exists_ReturnsOk()
        {
            var spot = new DockingSpot { Id = 3 };
            _dockServiceMock.Setup(s => s.GetDockingSpotById(3)).Returns(spot);

            var result = _controller.GetDockingSpot(3);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<DockingSpotReturnDto>(ok.Value);
            Assert.Equal(3, dto.Id);
        }

        [Fact]
        public void GetDockingSpot_NotFound_ReturnsNotFound()
        {
            _dockServiceMock.Setup(s => s.GetDockingSpotById(99)).Returns((DockingSpot)null);

            var result = _controller.GetDockingSpot(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
