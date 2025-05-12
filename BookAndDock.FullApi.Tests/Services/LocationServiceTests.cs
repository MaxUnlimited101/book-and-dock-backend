// File: Backend.Tests/Services/LocationServiceTests.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepository> _locationRepoMock;
        private readonly Mock<IPortRepository> _portRepoMock;
        private readonly Mock<IDockingSpotRepository> _dockRepoMock;
        private readonly LocationService _svc;

        public LocationServiceTests()
        {
            _locationRepoMock = new Mock<ILocationRepository>();
            _portRepoMock = new Mock<IPortRepository>();
            _dockRepoMock = new Mock<IDockingSpotRepository>();
            _svc = new LocationService(
                _locationRepoMock.Object,
                _portRepoMock.Object,
                _dockRepoMock.Object
            );
        }

        [Fact]
        public async Task CreateLocationAsync_PortAndDockNull_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", null, null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateLocationAsync(dto));
            Assert.Equal("Location must have either PortId or DockingSpotId set.", ex.Message);
        }

        [Fact]
        public async Task CreateLocationAsync_BothPortAndDockSet_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", 1, 2);
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port { Id = 1 });
            _dockRepoMock.Setup(d => d.GetDockingSpotById(2)).Returns(new DockingSpot { Id = 2 });
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateLocationAsync(dto));
            Assert.Equal("Location cannot have both PortId and DockingSpotId set.", ex.Message);
        }

        [Fact]
        public async Task CreateLocationAsync_PortSetButNotFound_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", 5, null);
            _portRepoMock.Setup(p => p.GetById(5)).Returns((Port?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateLocationAsync(dto));
            Assert.Equal("Port with ID 5 not found.", ex.Message);
        }

        [Fact]
        public async Task CreateLocationAsync_DockSetButNotFound_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", null, 5);
            _dockRepoMock.Setup(d => d.GetDockingSpotById(5)).Returns((DockingSpot?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateLocationAsync(dto));
            Assert.Equal("Docking spot with ID 5 not found.", ex.Message);
        }

        [Fact]
        public async Task DeleteLocationAsync_NotFound_Throws()
        {
            _locationRepoMock.Setup(l => l.GetLocationByIdAsync(3)).ReturnsAsync((Location?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.DeleteLocationAsync(3));
            Assert.Equal("Location with ID 3 not found.", ex.Message);
        }

        [Fact]
        public async Task DeleteLocationAsync_Exists_CallsDelete()
        {
            _locationRepoMock.Setup(l => l.GetLocationByIdAsync(4)).ReturnsAsync(new Location { Id = 4 });
            await _svc.DeleteLocationAsync(4);
            _locationRepoMock.Verify(l => l.DeleteLocationAsync(4), Times.Once);
        }

        [Fact]
        public async Task GetAllLocationsAsync_ReturnsMappedDtos()
        {
            var list = new List<Location>
            {
                new Location { Id = 1, Latitude = 1, Longitude = 2, Town = "Town1" },
                new Location { Id = 2, Latitude = 3, Longitude = 4, Town = "Town2" }
            };
            _locationRepoMock.Setup(l => l.GetAllLocationsAsync()).ReturnsAsync(list);
            var result = (await _svc.GetAllLocationsAsync()).ToList();
            Assert.Equal(2, result.Count);
            Assert.Equal(list[0].Id, result[0].Id);
        }

        [Fact]
        public async Task GetLocationByIdAsync_NotFound_Throws()
        {
            _locationRepoMock.Setup(l => l.GetLocationByIdAsync(6)).ReturnsAsync((Location?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.GetLocationByIdAsync(6));
            Assert.Equal("Location with ID 6 not found.", ex.Message);
        }

        [Fact]
        public async Task GetLocationByIdAsync_Exists_ReturnsDto()
        {
            var loc = new Location { Id = 7, Latitude = 5, Longitude = 6, Town = "Town7" };
            _locationRepoMock.Setup(l => l.GetLocationByIdAsync(7)).ReturnsAsync(loc);
            var result = await _svc.GetLocationByIdAsync(7);
            Assert.NotNull(result);
            Assert.Equal(loc.Id, result!.Id);
        }

        [Fact]
        public async Task UpdateLocationAsync_PortAndDockNull_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", null, null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdateLocationAsync(dto));
            Assert.Equal("Location must have either PortId or DockingSpotId set.", ex.Message);
        }

        [Fact]
        public async Task UpdateLocationAsync_BothPortAndDockSet_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", 1, 2);
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port { Id = 1 });
            _dockRepoMock.Setup(d => d.GetDockingSpotById(2)).Returns(new DockingSpot { Id = 2 });
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdateLocationAsync(dto));
            Assert.Equal("Location cannot have both PortId and DockingSpotId set.", ex.Message);
        }

        [Fact]
        public async Task UpdateLocationAsync_PortSetButNotFound_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", 5, null);
            _portRepoMock.Setup(p => p.GetById(5)).Returns((Port?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdateLocationAsync(dto));
            Assert.Equal("Port with ID 5 not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateLocationAsync_DockSetButNotFound_Throws()
        {
            var dto = new LocationDto(0, null, 1, 1, "Town", null, 5);
            _dockRepoMock.Setup(d => d.GetDockingSpotById(5)).Returns((DockingSpot?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdateLocationAsync(dto));
            Assert.Equal("Docking spot with ID 5 not found.", ex.Message);
        }
    }
}
