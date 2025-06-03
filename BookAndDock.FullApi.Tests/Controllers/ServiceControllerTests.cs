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
    public class ServiceControllerTests
    {
        private readonly Mock<IServiceService> _serviceServiceMock;
        private readonly ServiceController _controller;

        public ServiceControllerTests()
        {
            _serviceServiceMock = new Mock<IServiceService>();
            _controller = new ServiceController(_serviceServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsList()
        {
            var dto = new ServiceDto(1, "Wash", "Boat wash", 100, 1, 2, true, DateTime.UtcNow);
            var service = new Service
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                PortId = dto.PortId,
                DockingSpotId = dto.DockingSpotId,
                IsAvailable = dto.IsAvailable,
                CreatedOn = dto.CreatedOn
            };
            _serviceServiceMock.Setup(s => s.GetAllServicesAsync()).ReturnsAsync(new List<Service> { service });

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<ServiceDto>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task GetById_ReturnsService()
        {
            var dto = new ServiceDto(1, "Wash", "Boat wash", 100, 1, 2, true, DateTime.UtcNow);
            var service = new Service
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                PortId = dto.PortId,
                DockingSpotId = dto.DockingSpotId,
                IsAvailable = dto.IsAvailable,
                CreatedOn = dto.CreatedOn
            };
            _serviceServiceMock.Setup(s => s.GetServiceByIdAsync(1)).ReturnsAsync(service);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ServiceDto>(okResult.Value);
            Assert.Equal(1, returned.Id);
        }
    }
}
