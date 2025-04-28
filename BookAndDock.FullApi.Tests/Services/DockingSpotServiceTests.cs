// File: Backend.Tests/Services/DockingSpotServiceTests.cs

using System;
using System.Collections.Generic;
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
    public class DockingSpotServiceTests
    {
        private readonly Mock<IDockingSpotRepository> _dockRepoMock;
        private readonly Mock<IPortRepository> _portRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly DockingSpotService _svc;

        public DockingSpotServiceTests()
        {
            _dockRepoMock = new Mock<IDockingSpotRepository>();
            _portRepoMock = new Mock<IPortRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _svc = new DockingSpotService(
                _dockRepoMock.Object,
                _portRepoMock.Object,
                _userRepoMock.Object
            );
        }

        [Fact]
        public async Task GetAvailableDockingSpotsAsync_ReturnsListFromRepository()
        {
            // Arrange
            var list = new List<DockingSpot> { new DockingSpot { Id = 1 }, new DockingSpot { Id = 2 } };
            _dockRepoMock
                .Setup(r => r.GetAvailableDockingSpotsAsync(null, null, null, null, null))
                .ReturnsAsync(list);

            // Act
            var result = await _svc.GetAvailableDockingSpotsAsync(null, null, null, null, null);

            // Assert
            Assert.Equal(list, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckIfDockingSpotExistsById_ReturnsRepositoryValue(bool exists)
        {
            // Arrange
            _dockRepoMock.Setup(r => r.CheckIfDockingSpotExistsById(5)).Returns(exists);

            // Act
            var result = _svc.CheckIfDockingSpotExistsById(5);

            // Assert
            Assert.Equal(exists, result);
        }

        [Fact]
        public void GetDockingSpotById_NotFound_ThrowsModelInvalidException()
        {
            // Arrange
            _dockRepoMock.Setup(r => r.GetDockingSpotById(3)).Returns((DockingSpot?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.GetDockingSpotById(3));
            Assert.Equal("DockingSpot not found", ex.Message);
        }

        [Fact]
        public void GetDockingSpotById_Exists_ReturnsDock()
        {
            // Arrange
            var dock = new DockingSpot { Id = 7 };
            _dockRepoMock.Setup(r => r.GetDockingSpotById(7)).Returns(dock);

            // Act
            var result = _svc.GetDockingSpotById(7);

            // Assert
            Assert.Equal(dock, result);
        }

        [Fact]
        public void CreateDockingSpot_OwnerNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new DockingSpotDto(
                Id: 0,
                Name: "Name",
                Description: "Desc",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 10,
                PricePerPerson: 5,
                IsAvailable: true,
                CreatedOn: DateTime.UtcNow
            );
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns((User?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.CreateDockingSpot(dto));
            Assert.Equal("Owner not found", ex.Message);
        }

        [Fact]
        public void CreateDockingSpot_PortNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new DockingSpotDto(
                Id: 0,
                Name: "Name",
                Description: "Desc",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 10,
                PricePerPerson: 5,
                IsAvailable: true,
                CreatedOn: DateTime.UtcNow
            );
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns(new User { Id = dto.OwnerId, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" });
            _portRepoMock.Setup(r => r.GetPortById(dto.PortId)).Returns((Port?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.CreateDockingSpot(dto));
            Assert.Equal("Port not found", ex.Message);
        }

        [Fact]
        public void CreateDockingSpot_Valid_CallsRepositoryCreateWithMappedEntity()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var dto = new DockingSpotDto(
                Id: 0,
                Name: "Spot",
                Description: "D",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 20,
                PricePerPerson: 3,
                IsAvailable: false,
                CreatedOn: now
            );
            var user = new User { Id = dto.OwnerId, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" };
            var port = new Port { Id = dto.PortId, Name = "Port", OwnerId = 3, Description = "d", IsApproved = true };
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns(user);
            _portRepoMock.Setup(r => r.GetPortById(dto.PortId)).Returns(port);

            // Act
            _svc.CreateDockingSpot(dto);

            // Assert
            _dockRepoMock.Verify(r => r.CreateDockingSpot(
                It.Is<DockingSpot>(d =>
                    d.OwnerId == dto.OwnerId &&
                    d.Owner == user &&
                    d.PortId == dto.PortId &&
                    d.Port == port &&
                    d.CreatedOn == now &&
                    d.Name == dto.Name &&
                    d.Description == dto.Description &&
                    d.IsAvailable == dto.IsAvailable &&
                    d.PricePerNight == dto.PricePerNight &&
                    d.PricePerPerson == dto.PricePerPerson
                )
            ), Times.Once);
        }

        [Fact]
        public void UpdateDockingSpot_NotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new DockingSpotDto(
                Id: 5,
                Name: "N",
                Description: "D",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 5,
                PricePerPerson: 2,
                IsAvailable: true,
                CreatedOn: DateTime.UtcNow
            );
            _dockRepoMock.Setup(r => r.GetDockingSpotById(dto.Id)).Returns((DockingSpot?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateDockingSpot(dto));
            Assert.Equal("DockingSpot not found", ex.Message);
        }

        [Fact]
        public void UpdateDockingSpot_OwnerNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new DockingSpotDto(
                Id: 6,
                Name: "N",
                Description: "D",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 5,
                PricePerPerson: 2,
                IsAvailable: true,
                CreatedOn: DateTime.UtcNow
            );
            _dockRepoMock.Setup(r => r.GetDockingSpotById(dto.Id)).Returns(new DockingSpot { Id = dto.Id });
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns((User?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateDockingSpot(dto));
            Assert.Equal("Owner not found", ex.Message);
        }

        [Fact]
        public void UpdateDockingSpot_PortNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new DockingSpotDto(
                Id: 7,
                Name: "N",
                Description: "D",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 5,
                PricePerPerson: 2,
                IsAvailable: true,
                CreatedOn: DateTime.UtcNow
            );
            var existing = new DockingSpot { Id = dto.Id };
            _dockRepoMock.Setup(r => r.GetDockingSpotById(dto.Id)).Returns(existing);
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns(new User { Id = dto.OwnerId, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" });
            _portRepoMock.Setup(r => r.GetPortById(dto.PortId)).Returns((Port?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateDockingSpot(dto));
            Assert.Equal("Port not found", ex.Message);
        }

        [Fact]
        public void UpdateDockingSpot_Valid_CallsRepositoryUpdateWithMappedEntity()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var dto = new DockingSpotDto(
                Id: 8,
                Name: "Spot",
                Description: "Desc",
                OwnerId: 1,
                PortId: 2,
                PricePerNight: 15,
                PricePerPerson: 4,
                IsAvailable: false,
                CreatedOn: now
            );
            var existing = new DockingSpot { Id = dto.Id, Name = "Old", Description = "OldDesc", CreatedOn = now.AddDays(-1), IsAvailable = true };
            var user = new User { Id = dto.OwnerId, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" };
            var port = new Port { Id = dto.PortId, Name = "Port", OwnerId = 3, Description = "d", IsApproved = false };
            _dockRepoMock.Setup(r => r.GetDockingSpotById(dto.Id)).Returns(existing);
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns(user);
            _portRepoMock.Setup(r => r.GetPortById(dto.PortId)).Returns(port);

            // Act
            _svc.UpdateDockingSpot(dto);

            // Assert
            _dockRepoMock.Verify(r => r.UpdateDockingSpot(
                It.Is<DockingSpot>(d =>
                    d.Id == dto.Id &&
                    d.OwnerId == dto.OwnerId &&
                    d.Owner == user &&
                    d.PortId == dto.PortId &&
                    d.Port == port &&
                    d.Name == dto.Name &&
                    d.Description == dto.Description &&
                    d.CreatedOn == now &&
                    d.IsAvailable == dto.IsAvailable &&
                    d.PricePerNight == dto.PricePerNight &&
                    d.PricePerPerson == dto.PricePerPerson
                )
            ), Times.Once);
        }

        [Fact]
        public void DeleteDockingSpot_NotFound_ThrowsModelInvalidException()
        {
            // Arrange
            _dockRepoMock.Setup(r => r.GetDockingSpotById(9)).Returns((DockingSpot?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.DeleteDockingSpot(9));
            Assert.Equal("Dock not found", ex.Message);
        }

        [Fact]
        public void DeleteDockingSpot_Valid_CallsRepositoryDelete()
        {
            // Arrange
            _dockRepoMock.Setup(r => r.GetDockingSpotById(10)).Returns(new DockingSpot { Id = 10 });

            // Act
            _svc.DeleteDockingSpot(10);

            // Assert
            _dockRepoMock.Verify(r => r.DeleteDockingSpot(10), Times.Once);
        }
    }
}
