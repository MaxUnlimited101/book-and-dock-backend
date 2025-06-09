using System.Collections.Generic;
using System.Security.Claims;
using Backend.Controllers;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class PortControllerTests
    {
        private readonly Mock<IPortService> _portServiceMock;
        private readonly Mock<IPortRepository> _portRepoMock;
        private readonly Mock<IBookingRepository> _bookingRepoMock;
        private readonly PortController _controller;

        public PortControllerTests()
        {
            _portServiceMock = new Mock<IPortService>();
            _portRepoMock = new Mock<IPortRepository>();
            _bookingRepoMock = new Mock<IBookingRepository>();
            _controller = new PortController(
                _portServiceMock.Object,
                _portRepoMock.Object,
                _bookingRepoMock.Object
            );
        }

        

        

        [Fact]
        public void UpdatePort_WhenNotFound_ReturnsNotFound()
        {
            _portRepoMock.Setup(r => r.GetById(1)).Returns((Port)null);

            var result = _controller.UpdatePort(1, new PortDto(
                Id: 0,
                Name: "Harbor A",
                Description: "Nice place",
                IsApproved: true,
                OwnerId: 1,
                CreatedOn: DateTime.UtcNow)
            );

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDockOwnerBookings_WhenUserIdPresent_ReturnsOk()
        {
            var bookings = new List<Booking> { new Booking { Id = 1 } };
            _bookingRepoMock.Setup(b => b.GetBookingsByDockOwnerId(1)).Returns(bookings);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = _controller.GetDockOwnerBookings();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Single((IEnumerable<Booking>)ok.Value);
        }

        [Fact]
        public void GetDockOwnerBookings_WhenUserIdMissing_ReturnsUnauthorized()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = _controller.GetDockOwnerBookings();

            Assert.IsType<UnauthorizedResult>(result);
        }

        

        [Fact]
        public void GetPortById_WhenNotFound_ReturnsNotFound()
        {
            _portRepoMock.Setup(r => r.GetById(1)).Returns((Port)null);

            var result = _controller.GetPortById(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
