using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class LocationControllerTests
    {
        private readonly Mock<ILocationRepository> _locationRepoMock;
        private readonly Mock<IPortRepository> _portRepoMock;
        private readonly Mock<IDockingSpotRepository> _dockRepoMock;
        private readonly LocationService _locationService;
        private readonly LocationController _controller;

        public LocationControllerTests()
        {
            _locationRepoMock = new Mock<ILocationRepository>();
            _portRepoMock = new Mock<IPortRepository>();
            _dockRepoMock = new Mock<IDockingSpotRepository>();
            _locationService = new LocationService(_locationRepoMock.Object, _portRepoMock.Object, _dockRepoMock.Object);
            _controller = new LocationController(_locationService);
        }

        [Fact]
        public async Task GetAllLocations_WhenNoneExist_ReturnsNotFound()
        {
            _locationRepoMock.Setup(r => r.GetAllLocationsAsync()).ReturnsAsync(new List<Location>());

            var result = await _controller.GetAllLocations();

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No locations found.", notFound.Value);
        }

        [Fact]
        public async Task GetAllLocations_WhenSomeExist_ReturnsOk()
        {
            var locations = new List<Location> { new Location { Id = 1, Latitude = 10, Longitude = 20, Town = "TestTown", PortId = 1 } };
            _locationRepoMock.Setup(r => r.GetAllLocationsAsync()).ReturnsAsync(locations);

            var result = await _controller.GetAllLocations();

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(ok.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task GetLocationById_WhenExists_ReturnsOk()
        {
            var location = new Location { Id = 1, Latitude = 10, Longitude = 20, Town = "TestTown", PortId = 1 };
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1)).ReturnsAsync(location);

            var result = await _controller.GetLocationById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<LocationDto>(ok.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public async Task GetLocationById_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1))
                .ThrowsAsync(new ModelInvalidException("Location with ID 1 not found."));

            // Act
            var result = await Record.ExceptionAsync(() => _controller.GetLocationById(1));

            // Assert
            var exception = Assert.IsType<ModelInvalidException>(result);
            Assert.Equal("Location with ID 1 not found.", exception.Message);
        }


        [Fact]
        public async Task CreateLocation_Valid_ReturnsCreated()
        {
            var dto = new LocationDto(1, null, 10, 20, "Town", 1, null);
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port());

            var result = await _controller.CreateLocation(dto);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task CreateLocation_Invalid_ReturnsBadRequest()
        {
            var dto = new LocationDto(1, null, 10, 20, "Town", 1, 2);
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port());
            _dockRepoMock.Setup(d => d.GetDockingSpotById(2)).Returns(new DockingSpot());

            var result = await _controller.CreateLocation(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.StartsWith("Error creating location:", bad.Value.ToString());
        }

        [Fact]
        public async Task UpdateLocation_Valid_ReturnsOk()
        {
            var dto = new LocationDto(1, null, 10, 20, "Town", 1, null);
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1)).ReturnsAsync(new Location { Id = 1 });
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port());

            var result = await _controller.UpdateLocation(dto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateLocation_NotFound_ReturnsNotFound()
        {
            var dto = new LocationDto(1, null, 10, 20, "Town", 1, null);
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1)).ThrowsAsync(new ModelInvalidException("Location with ID 1 not found."));

            var result = await _controller.UpdateLocation(dto);

            var notFound = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error updating location: Location with ID 1 not found.", notFound.Value);
        }

        [Fact]
        public async Task UpdateLocation_Invalid_ReturnsBadRequest()
        {
            var dto = new LocationDto(1, null, 10, 20, "Town", 1, 2);
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1)).ReturnsAsync(new Location { Id = 1 });
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port());
            _dockRepoMock.Setup(d => d.GetDockingSpotById(2)).Returns(new DockingSpot());

            var result = await _controller.UpdateLocation(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.StartsWith("Error updating location:", bad.Value.ToString());
        }

        [Fact]
        public async Task DeleteLocation_Valid_ReturnsOk()
        {
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1)).ReturnsAsync(new Location { Id = 1 });

            var result = await _controller.DeleteLocation(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteLocation_Invalid_ReturnsBadRequest()
        {
            _locationRepoMock.Setup(r => r.GetLocationByIdAsync(1)).ReturnsAsync((Location)null);

            var result = await _controller.DeleteLocation(1);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.StartsWith("Error deleting location:", bad.Value.ToString());
        }
    }
}
