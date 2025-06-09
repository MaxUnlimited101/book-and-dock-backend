using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly Mock<IPaymentMethodService> _paymentServiceMock;
        private readonly BookingService _svc;

        public BookingServiceTests()
        {
            _bookingRepoMock = new Mock<IBookingRepository>();
            _userServiceMock = new Mock<IUserService>();
            _dockServiceMock = new Mock<IDockingSpotService>();
            _paymentServiceMock = new Mock<IPaymentMethodService>();

            _svc = new BookingService(
                _bookingRepoMock.Object,
                _userServiceMock.Object,
                _dockServiceMock.Object,
                _paymentServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateBooking_ValidInput_ReturnsCreatedId()
        {
            var dto = new CreateBookingDto(
                SailorId: 1,
                StartDate: DateTime.Today,
                EndDate: DateTime.Today.AddDays(1),
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );

            _userServiceMock.Setup(u => u.CheckIfUserExistsById(dto.SailorId)).Returns(true);
            _dockServiceMock.Setup(d => d.CheckIfDockingSpotExistsById(dto.DockingSpotId)).Returns(true);
            _paymentServiceMock.Setup(p => p.GetPaymentMethodByNameAsync(dto.Payment))
                .ReturnsAsync(new PaymentMethod { Id = 99, Name = dto.Payment });

            _bookingRepoMock.Setup(r => r.Create(It.IsAny<Booking>())).Returns(123);

            var result = await _svc.CreateAsync(dto);

            Assert.Equal(123, result);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public async Task CreateBooking_UserDoesNotExist_ThrowsModelNotFoundException()
        {
            var dto = new CreateBookingDto(
                SailorId: 1,
                StartDate: DateTime.Today,
                EndDate: DateTime.Today.AddDays(1),
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );

            _userServiceMock.Setup(u => u.CheckIfUserExistsById(dto.SailorId)).Returns(false);

            var ex = await Assert.ThrowsAsync<ModelNotFoundException>(() => _svc.CreateAsync(dto));
            Assert.Equal("Invalid user", ex.Message);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CreateBooking_DockDoesNotExist_ThrowsModelNotFoundException()
        {
            var dto = new CreateBookingDto(
                SailorId: 1,
                StartDate: DateTime.Today,
                EndDate: DateTime.Today.AddDays(1),
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );

            _userServiceMock.Setup(u => u.CheckIfUserExistsById(dto.SailorId)).Returns(true);
            _dockServiceMock.Setup(d => d.CheckIfDockingSpotExistsById(dto.DockingSpotId)).Returns(false);

            var ex = await Assert.ThrowsAsync<ModelNotFoundException>(() => _svc.CreateAsync(dto));
            Assert.Equal("Invalid dock", ex.Message);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Never);
        }

        [Theory]
        [InlineData(0, 0)] // end = start
        [InlineData(1, 0)] // end < start
        public async Task CreateBooking_InvalidDates_ThrowsInvalidDataException(int startOffset, int endOffset)
        {
            var start = DateTime.Today.AddDays(startOffset);
            var end = DateTime.Today.AddDays(endOffset);

            var dto = new CreateBookingDto(
                SailorId: 1,
                StartDate: start,
                EndDate: end,
                People: 2,
                Payment: "credit",
                DockingSpotId: 5
            );

            _userServiceMock.Setup(u => u.CheckIfUserExistsById(dto.SailorId)).Returns(true);
            _dockServiceMock.Setup(d => d.CheckIfDockingSpotExistsById(dto.DockingSpotId)).Returns(true);

            var ex = await Assert.ThrowsAsync<InvalidDataException>(() => _svc.CreateAsync(dto));
            Assert.Equal("End date cannot be earlier than start date", ex.Message);
            _bookingRepoMock.Verify(r => r.Create(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public void Delete_ExistingBooking_DeletesSuccessfully()
        {
            var id = 10;
            _bookingRepoMock.Setup(r => r.CheckIfExistsById(id)).Returns(true);

            _svc.Delete(id);

            _bookingRepoMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public void Delete_NonExistingBooking_ThrowsModelNotFoundException()
        {
            var id = 20;
            _bookingRepoMock.Setup(r => r.CheckIfExistsById(id)).Returns(false);

            var ex = Assert.Throws<ModelNotFoundException>(() => _svc.Delete(id));
            Assert.Equal("Invalid booking id", ex.Message);
        }
    }
}
