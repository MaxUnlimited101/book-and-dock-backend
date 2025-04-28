using System;
using System.Collections.Generic;
using System.Net;
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
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepoMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IDockingSpotService> _dockServiceMock;
        private readonly BookingService _svc;

        public BookingServiceTests()
        {
            // Arrange mocks and service under test
            _bookingRepoMock = new Mock<IBookingRepository>();
            _userServiceMock = new Mock<IUserService>();
            _dockServiceMock = new Mock<IDockingSpotService>();
            _svc = new BookingService(
                _bookingRepoMock.Object,
                _userServiceMock.Object,
                _dockServiceMock.Object
            );
        }

        [Fact]
        public void Create_ValidInput_ReturnsCreatedId()
        {
            // Arrange
            var dto = new CreateBookingDto(
                SailorId: 1,
                DockId: 0,               // unused by service
                StartDate: DateOnly.FromDateTime(DateTime.Today),
                EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );
            _userServiceMock
                .Setup(u => u.CheckIfUserExistsByIdAndRole(dto.SailorId, It.IsAny<Role>()))
                .Returns(true);
            _dockServiceMock
                .Setup(d => d.CheckIfDockingSpotExistsById(dto.DockingSpotId))
                .Returns(true);
            _bookingRepoMock
                .Setup(r => r.Create(It.IsAny<Booking>()))
                .Returns(123);

            // Act
            var result = _svc.Create(dto);

            // Assert
            Assert.Equal(123, result);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public void Create_UserDoesNotExist_ThrowsModelNotFoundException()
        {
            // Arrange
            var dto = new CreateBookingDto(
                SailorId: 1,
                DockId: 0,
                StartDate: DateOnly.FromDateTime(DateTime.Today),
                EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );
            _userServiceMock
                .Setup(u => u.CheckIfUserExistsByIdAndRole(dto.SailorId, It.IsAny<Role>()))
                .Returns(false);

            // Act & Assert
            var ex = Assert.Throws<ModelNotFoundException>(() => _svc.Create(dto));
            Assert.Equal("Invalid user", ex.Message);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void Create_DockDoesNotExist_ThrowsModelNotFoundException()
        {
            // Arrange
            var dto = new CreateBookingDto(
                SailorId: 1,
                DockId: 0,
                StartDate: DateOnly.FromDateTime(DateTime.Today),
                EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );
            _userServiceMock
                .Setup(u => u.CheckIfUserExistsByIdAndRole(dto.SailorId, It.IsAny<Role>()))
                .Returns(true);
            _dockServiceMock
                .Setup(d => d.CheckIfDockingSpotExistsById(dto.DockingSpotId))
                .Returns(false);

            // Act & Assert
            var ex = Assert.Throws<ModelNotFoundException>(() => _svc.Create(dto));
            Assert.Equal("Invalid dock", ex.Message);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Never);
        }

        [Theory]
        [InlineData(0, 0)]  // end same as start
        [InlineData(1, 0)]  // end before start
        public void Create_InvalidDates_ThrowsInvalidDataException(int startOffset, int endOffset)
        {
            // Arrange
            var start = DateOnly.FromDateTime(DateTime.Today.AddDays(startOffset));
            var end = DateOnly.FromDateTime(DateTime.Today.AddDays(endOffset));
            var dto = new CreateBookingDto(
                SailorId: 1,
                DockId: 0,
                StartDate: start,
                EndDate: end,
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );
            _userServiceMock
                .Setup(u => u.CheckIfUserExistsByIdAndRole(dto.SailorId, It.IsAny<Role>()))
                .Returns(true);
            _dockServiceMock
                .Setup(d => d.CheckIfDockingSpotExistsById(dto.DockingSpotId))
                .Returns(true);

            // Act & Assert
            var ex = Assert.Throws<InvalidDataException>(() => _svc.Create(dto));
            Assert.Equal("End date cannot be earlier than start date", ex.Message);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void Delete_ExistingBooking_DeletesSuccessfully()
        {
            // Arrange
            var id = 10;
            _bookingRepoMock
                .Setup(r => r.CheckIfExistsById(id))
                .Returns(true);

            // Act
            _svc.Delete(id);

            // Assert
            _bookingRepoMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public void Delete_NonExistingBooking_ThrowsModelNotFoundException()
        {
            // Arrange
            var id = 20;
            _bookingRepoMock
                .Setup(r => r.CheckIfExistsById(id))
                .Returns(false);

            // Act & Assert
            var ex = Assert.Throws<ModelNotFoundException>(() => _svc.Delete(id));
            Assert.Equal("Invalid booking id", ex.Message);
        }
    }
}
