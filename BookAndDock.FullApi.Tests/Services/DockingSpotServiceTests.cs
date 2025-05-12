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
            var list = new List<DockingSpot> { new DockingSpot { Id = 1 }, new DockingSpot { Id = 2 } };
            _dockRepoMock
                .Setup(r => r.GetAvailableDockingSpotsAsync(null, null, null, null, null))
                .ReturnsAsync(list);

            var result = await _svc.GetAvailableDockingSpotsAsync(null, null, null, null, null);

            Assert.Equal(list, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckIfDockingSpotExistsById_ReturnsRepositoryValue(bool exists)
        {
            _dockRepoMock.Setup(r => r.CheckIfDockingSpotExistsById(5)).Returns(exists);
            var result = _svc.CheckIfDockingSpotExistsById(5);
            Assert.Equal(exists, result);
        }

        [Fact]
        public void GetDockingSpotById_NotFound_ThrowsModelInvalidException()
        {
            _dockRepoMock.Setup(r => r.GetDockingSpotById(3)).Returns((DockingSpot?)null);
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.GetDockingSpotById(3));
            Assert.Equal("DockingSpot not found", ex.Message);
        }

        [Fact]
        public void GetDockingSpotById_Exists_ReturnsDock()
        {
            var dock = new DockingSpot { Id = 7 };
            _dockRepoMock.Setup(r => r.GetDockingSpotById(7)).Returns(dock);
            var result = _svc.GetDockingSpotById(7);
            Assert.Equal(dock, result);
        }

        [Fact]
        public void CreateDockingSpot_OwnerNotFound_ThrowsModelInvalidException()
        {
            var dto = new DockingSpotDto(0, "Name", "Desc", 1, 2, 10, 5, true, DateTime.UtcNow);
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns((User?)null);
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.CreateDockingSpot(dto));
            Assert.Equal("Owner not found", ex.Message);
        }

        [Fact]
        public void CreateDockingSpot_Valid_CallsRepositoryCreateWithMappedEntity()
        {
            var now = DateTime.UtcNow;
            var dto = new DockingSpotDto(0, "Spot", "D", 1, 2, 20, 3, false, now);
            var user = new User { Id = dto.OwnerId, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" };
            var port = new Port { Id = dto.PortId, Name = "Port", OwnerId = 3, Description = "d", IsApproved = true };
            _userRepoMock.Setup(r => r.GetUserById(dto.OwnerId)).Returns(user);
            _portRepoMock.Setup(r => r.GetById(dto.PortId)).Returns(port);

            _svc.CreateDockingSpot(dto);

            _dockRepoMock.Verify(r => r.CreateDockingSpot(
                It.Is<DockingSpot>(d =>
                    d.OwnerId == dto.OwnerId &&
                    d.Owner == user &&
                    d.PortId == dto.PortId &&
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
        public void DeleteDockingSpot_NotFound_ThrowsModelInvalidException()
        {
            _dockRepoMock.Setup(r => r.GetDockingSpotById(9)).Returns((DockingSpot?)null);
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.DeleteDockingSpot(9));
            Assert.Equal("Dock not found", ex.Message);
        }

        [Fact]
        public void DeleteDockingSpot_Valid_CallsRepositoryDelete()
        {
            _dockRepoMock.Setup(r => r.GetDockingSpotById(10)).Returns(new DockingSpot { Id = 10 });
            _svc.DeleteDockingSpot(10);
            _dockRepoMock.Verify(r => r.DeleteDockingSpot(10), Times.Once);
        }
    }
}
