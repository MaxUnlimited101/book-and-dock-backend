using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests.Repositories
{
    public class LocationRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _dbOptions;

        public LocationRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private Location CreateSampleLocation(int id = 1)
        {
            return new Location
            {
                Id = id,
                Latitude = 1.23,
                Longitude = 4.56,
                Town = "Sample Town"
            };
        }

        [Fact]
        public async Task CreateLocationAsync_AddsLocationToDb()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new LocationRepository(context);
            var location = CreateSampleLocation();

            await repo.CreateLocationAsync(location);

            var fromDb = await context.Locations.FindAsync(location.Id);
            Assert.NotNull(fromDb);
            Assert.Equal(location.Town, fromDb!.Town);
        }

        [Fact]
        public async Task DeleteLocationAsync_RemovesIfExists()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var location = CreateSampleLocation();
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var repo = new LocationRepository(context);
            await repo.DeleteLocationAsync(location.Id);

            var deleted = await context.Locations.FindAsync(location.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteLocationAsync_NoOpIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new LocationRepository(context);

            var exception = await Record.ExceptionAsync(() => repo.DeleteLocationAsync(999));
            Assert.Null(exception); // should silently complete
        }

        [Fact]
        public async Task GetAllLocationsAsync_ReturnsAll()
        {
            using var context = new BookAndDockContext(_dbOptions);
            context.Locations.AddRange(new[]
            {
                CreateSampleLocation(1),
                CreateSampleLocation(2)
            });
            await context.SaveChangesAsync();

            var repo = new LocationRepository(context);
            var result = (await repo.GetAllLocationsAsync()).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetLocationByIdAsync_ReturnsLocation()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var location = CreateSampleLocation();
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var repo = new LocationRepository(context);
            var result = await repo.GetLocationByIdAsync(location.Id);

            Assert.NotNull(result);
            Assert.Equal(location.Id, result!.Id);
        }

        [Fact]
        public async Task GetLocationByIdAsync_ReturnsNullIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new LocationRepository(context);
            var result = await repo.GetLocationByIdAsync(1234);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateLocationAsync_UpdatesValues()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var location = CreateSampleLocation();
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            location.Town = "Updated Town";
            var repo = new LocationRepository(context);
            await repo.UpdateLocationAsync(location);

            var updated = await context.Locations.FindAsync(location.Id);
            Assert.Equal("Updated Town", updated!.Town);
        }
    }
}
