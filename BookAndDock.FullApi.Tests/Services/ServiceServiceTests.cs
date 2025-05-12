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
    public class ServiceServiceTests
    {
        private readonly Mock<IServiceRepository> _repoMock;
        private readonly Mock<IPortRepository> _portRepoMock;
        private readonly Mock<IDockingSpotRepository> _dockRepoMock;
        private readonly ServiceService _svc;

        public ServiceServiceTests()
        {
            _repoMock = new Mock<IServiceRepository>();
            _portRepoMock = new Mock<IPortRepository>();
            _dockRepoMock = new Mock<IDockingSpotRepository>();
            _svc = new ServiceService(_repoMock.Object, _portRepoMock.Object, _dockRepoMock.Object);
        }

        [Fact]
        public async Task GetAllServicesAsync_ReturnsServices()
        {
            var list = new List<Service> { new Service { Id = 1 }, new Service { Id = 2 } };
            _repoMock.Setup(r => r.GetAllServicesAsync()).ReturnsAsync(list);
            var result = await _svc.GetAllServicesAsync();
            Assert.Equal(list, result);
        }

        [Fact]
        public async Task GetServiceByIdAsync_Exists_ReturnsService()
        {
            var service = new Service { Id = 5 };
            _repoMock.Setup(r => r.GetServiceByIdAsync(5)).ReturnsAsync(service);
            var result = await _svc.GetServiceByIdAsync(5);
            Assert.Equal(service, result);
        }

        [Fact]
        public async Task GetServiceByIdAsync_NotFound_Throws()
        {
            _repoMock.Setup(r => r.GetServiceByIdAsync(9)).ReturnsAsync((Service?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.GetServiceByIdAsync(9));
            Assert.Equal("Service not found", ex.Message);
        }

        [Fact]
        public async Task CreateServiceAsync_PortAndDockExist_ReturnsId()
        {
            var dto = new ServiceDto(0, "Cleaning", "Basic", 50, 1, 2, true, DateTime.UtcNow);
            _dockRepoMock.Setup(d => d.GetDockingSpotById(2)).Returns(new DockingSpot { Id = 2 });
            _portRepoMock.Setup(p => p.GetById(1)).Returns(new Port { Id = 1 });
            _repoMock.Setup(r => r.CreateServiceAsync(It.IsAny<Service>())).ReturnsAsync(10);

            var result = await _svc.CreateServiceAsync(dto);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task CreateServiceAsync_PortNotFound_Throws()
        {
            var dto = new ServiceDto(0, "Cleaning", "Basic", 50, 9, null, true, DateTime.UtcNow);
            _portRepoMock.Setup(p => p.GetById(9)).Returns((Port?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateServiceAsync(dto));
            Assert.Equal("Port not found", ex.Message);
        }

        [Fact]
        public async Task CreateServiceAsync_DockingSpotNotFound_Throws()
        {
            var dto = new ServiceDto(0, "Cleaning", "Basic", 50, null, 9, true, DateTime.UtcNow);
            _dockRepoMock.Setup(d => d.GetDockingSpotById(9)).Returns((DockingSpot?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateServiceAsync(dto));
            Assert.Equal("Docking spot not found", ex.Message);
        }

        [Fact]
        public async Task UpdateServiceAsync_Valid_UpdatesCorrectly()
        {
            var dto = new ServiceDto(1, "Update", "Desc", 70, 2, 3, false, DateTime.UtcNow);
            var existing = new Service { Id = 1 };
            _repoMock.Setup(r => r.GetServiceByIdAsync(1)).ReturnsAsync(existing);
            _portRepoMock.Setup(p => p.GetById(2)).Returns(new Port { Id = 2 });
            _dockRepoMock.Setup(d => d.GetDockingSpotById(3)).Returns(new DockingSpot { Id = 3 });

            await _svc.UpdateServiceAsync(dto);

            _repoMock.Verify(r => r.UpdateServiceAsync(It.Is<Service>(s =>
                s.Name == dto.Name && s.Price == dto.Price && s.PortId == dto.PortId
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateServiceAsync_NotFound_Throws()
        {
            var dto = new ServiceDto(2, "Name", "Desc", 20, null, null, true, null);
            _repoMock.Setup(r => r.GetServiceByIdAsync(2)).ReturnsAsync((Service?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdateServiceAsync(dto));
            Assert.Equal("Service not found", ex.Message);
        }

        [Fact]
        public async Task DeleteServiceAsync_Valid_CallsDelete()
        {
            _repoMock.Setup(r => r.GetServiceByIdAsync(6)).ReturnsAsync(new Service { Id = 6 });
            await _svc.DeleteServiceAsync(6);
            _repoMock.Verify(r => r.DeleteServiceAsync(6), Times.Once);
        }

        [Fact]
        public async Task DeleteServiceAsync_NotFound_Throws()
        {
            _repoMock.Setup(r => r.GetServiceByIdAsync(6)).ReturnsAsync((Service?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.DeleteServiceAsync(6));
            Assert.Equal("Service not found", ex.Message);
        }
    }
}
