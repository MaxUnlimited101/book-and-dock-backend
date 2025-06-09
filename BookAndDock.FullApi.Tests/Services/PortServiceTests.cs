using System;
using System.Collections.Generic;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class PortServiceTests
    {
        private readonly Mock<IPortRepository> _repoMock;
        private readonly PortService _svc;

        public PortServiceTests()
        {
            _repoMock = new Mock<IPortRepository>();
            _svc = new PortService(_repoMock.Object);
        }

        [Fact]
        public void Create_ValidDto_ReturnsNewId()
        {
            var dto = new PortDto(
                Id: 0,
                Name: "Harbor A",
                Description: "Nice place",
                IsApproved: true,
                OwnerId: 1,
                CreatedOn: DateTime.UtcNow
            );
            _repoMock.Setup(r => r.Create(It.IsAny<Port>())).Returns(42);

            var result = _svc.Create(dto);

            Assert.Equal(42, result);
            _repoMock.Verify(r => r.Create(It.Is<Port>(p =>
                p.Name == dto.Name &&
                p.Description == dto.Description &&
                p.IsApproved == dto.IsApproved &&
                p.OwnerId == 1
            )), Times.Once);
        }

        [Fact]
        public void Delete_PortExists_CallsDelete()
        {
            _repoMock.Setup(r => r.CheckIfExistsById(3)).Returns(true);

            _svc.Delete(3);

            _repoMock.Verify(r => r.Delete(3), Times.Once);
        }

        [Fact]
        public void Delete_PortNotExists_Throws()
        {
            _repoMock.Setup(r => r.CheckIfExistsById(7)).Returns(false);

            var ex = Assert.Throws<KeyNotFoundException>(() => _svc.Delete(7));

            Assert.Equal("Port with ID 7 not found.", ex.Message);
        }
    }
}
