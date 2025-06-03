using System.Collections.Generic;
using System.Linq;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class BookingRepositoryTests
    {
        private readonly BookingRepository _repository;
        private readonly BookAndDockContext _context;

        public BookingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: "BookingTestDb")
                .Options;
            _context = new BookAndDockContext(options);
            _repository = new BookingRepository(_context);
        }

        [Fact]
        public void Create_AddsBookingToDatabase()
        {
            var booking = new Booking { SailorId = 1, DockingSpotId = 1 };

            var id = _repository.Create(booking);

            var retrieved = _context.Bookings.Find(id);
            Assert.NotNull(retrieved);
            Assert.Equal(1, retrieved.SailorId);
        }

        [Fact]
        public void Delete_RemovesBookingFromDatabase()
        {
            var booking = new Booking { SailorId = 2, DockingSpotId = 2 };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            _repository.Delete(booking.Id);

            var result = _context.Bookings.Find(booking.Id);
            Assert.Null(result);
        }

        [Fact]
        public void CheckIfExistsById_ReturnsCorrectly()
        {
            var booking = new Booking { SailorId = 3, DockingSpotId = 3 };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            Assert.True(_repository.CheckIfExistsById(booking.Id));
            Assert.False(_repository.CheckIfExistsById(999));
        }

        [Fact]
        public void GetById_ReturnsBooking()
        {
            var booking = new Booking { SailorId = 4, DockingSpotId = 4 };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var result = _repository.GetById(booking.Id);

            Assert.NotNull(result);
            Assert.Equal(4, result.DockingSpotId);
        }


        [Fact]
        public void Update_ModifiesBooking()
        {
            var booking = new Booking { SailorId = 7, DockingSpotId = 7 };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            booking.DockingSpotId = 8;
            _repository.Update(booking);

            var updated = _context.Bookings.Find(booking.Id);
            Assert.Equal(8, updated.DockingSpotId);
        }
    }
}
