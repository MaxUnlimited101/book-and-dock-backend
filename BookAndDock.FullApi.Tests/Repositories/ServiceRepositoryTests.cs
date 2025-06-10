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
    public class ServiceRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _options;

        public ServiceRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private Service CreateSampleService(int id = 0, string name = "Water Supply")
        {
            return new Service
            {
                Id = id,
                Name = name,
                Description = "Standard service",
                Price = 50.0m,
                PortId = 1,
                DockingSpotId = 1,
                IsAvailable = true,
                CreatedOn = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task GetAllServicesAsync_ReturnsAllServices()
        {
            using var context = new BookAndDockContext(_options);
            context.Services.AddRange(
                CreateSampleService(1),
                CreateSampleService(2)
            );
            await context.SaveChangesAsync();

            var repo = new ServiceRepository(context);
            var result = (await repo.GetAllServicesAsync()).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetServiceByIdAsync_ReturnsCorrectService()
        {
            using var context = new BookAndDockContext(_options);
            var service = CreateSampleService(5, "Fuel Delivery");
            context.Services.Add(service);
            await context.SaveChangesAsync();

            var repo = new ServiceRepository(context);
            var result = await repo.GetServiceByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal("Fuel Delivery", result!.Name);
        }

        [Fact]
        public async Task GetServiceByIdAsync_ReturnsNullIfNotFound()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new ServiceRepository(context);

            var result = await repo.GetServiceByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateServiceAsync_AddsServiceAndReturnsId()
        {
            using var context = new BookAndDockContext(_options);
            var service = CreateSampleService();
            var repo = new ServiceRepository(context);

            var id = await repo.CreateServiceAsync(service);

            Assert.True(id > 0);
            var fromDb = await context.Services.FindAsync(id);
            Assert.NotNull(fromDb);
            Assert.Equal(service.Name, fromDb!.Name);
        }

        [Fact]
        public async Task UpdateServiceAsync_UpdatesEntity()
        {
            using var context = new BookAndDockContext(_options);
            var service = CreateSampleService(10, "Repair");
            context.Services.Add(service);
            await context.SaveChangesAsync();

            service.Price = 75.5m;
            service.IsAvailable = false;

            var repo = new ServiceRepository(context);
            await repo.UpdateServiceAsync(service);

            var updated = await context.Services.FindAsync(10);
            Assert.Equal(75.5m, updated!.Price);
            Assert.False(updated.IsAvailable);
        }

        [Fact]
        public async Task DeleteServiceAsync_RemovesIfExists()
        {
            using var context = new BookAndDockContext(_options);
            var service = CreateSampleService(20, "Garbage Disposal");
            context.Services.Add(service);
            await context.SaveChangesAsync();

            var repo = new ServiceRepository(context);
            await repo.DeleteServiceAsync(20);

            var deleted = await context.Services.FindAsync(20);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteServiceAsync_NoOpIfNotFound()
        {
            using var context = new BookAndDockContext(_options);
            var repo = new ServiceRepository(context);

            var ex = await Record.ExceptionAsync(() => repo.DeleteServiceAsync(999));
            Assert.Null(ex); // Should silently succeed
        }
    }
}
