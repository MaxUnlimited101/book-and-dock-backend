using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class DockingSpotRepositoryTests
    {
        private readonly BookAndDockContext _context;
        private readonly DockingSpotRepository _repository;

        public DockingSpotRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new BookAndDockContext(options);
            _repository = new DockingSpotRepository(_context);
        }

        [Fact]
        public void CreateDockingSpot_AddsSpot()
        {
            var dock = new DockingSpot { OwnerId = 1, PortId = 1, IsAvailable = true };

            var id = _repository.CreateDockingSpot(dock);

            Assert.True(id > 0);
        }

        [Fact]
        public void GetDockingSpotById_ReturnsSpot()
        {
            var dock = new DockingSpot { OwnerId = 2, PortId = 2, IsAvailable = true };
            var id = _repository.CreateDockingSpot(dock);

            var result = _repository.GetDockingSpotById(id);

            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public void GetAllDockingSpots_ReturnsAll()
        {
            _repository.CreateDockingSpot(new DockingSpot { OwnerId = 3, PortId = 3, IsAvailable = true });
            _repository.CreateDockingSpot(new DockingSpot { OwnerId = 4, PortId = 4, IsAvailable = false });

            var result = _repository.GetAllDockingSpots();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void CheckIfDockingSpotExistsById_ReturnsCorrectly()
        {
            var dock = new DockingSpot { OwnerId = 5, PortId = 5, IsAvailable = true };
            var id = _repository.CreateDockingSpot(dock);

            var exists = _repository.CheckIfDockingSpotExistsById(id);
            var notExists = _repository.CheckIfDockingSpotExistsById(999);

            Assert.True(exists);
            Assert.False(notExists);
        }

        [Fact]
        public void UpdateDockingSpot_ChangesPersist()
        {
            var dock = new DockingSpot { OwnerId = 6, PortId = 6, IsAvailable = true };
            var id = _repository.CreateDockingSpot(dock);

            dock = _repository.GetDockingSpotById(id)!;
            dock.IsAvailable = false;
            _repository.UpdateDockingSpot(dock);

            var updated = _repository.GetDockingSpotById(id);
            Assert.False(updated!.IsAvailable);
        }

        [Fact]
        public void DeleteDockingSpot_RemovesEntry()
        {
            var dock = new DockingSpot { OwnerId = 7, PortId = 7, IsAvailable = true };
            var id = _repository.CreateDockingSpot(dock);

            _repository.DeleteDockingSpot(id);

            var deleted = _repository.GetDockingSpotById(id);
            Assert.Null(deleted);
        }
    }
}
