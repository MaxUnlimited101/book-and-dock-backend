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
    public class PaymentMethodRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _dbOptions;

        public PaymentMethodRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private PaymentMethod CreateSamplePaymentMethod(int id = 0)
        {
            return new PaymentMethod
            {
                Id = id,
                Name = "Credit Card",
                CreatedOn = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task CreatePaymentMethodAsync_AddsToDatabaseAndReturnsId()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PaymentMethodRepository(context);
            var method = CreateSamplePaymentMethod();

            var id = await repo.CreatePaymentMethodAsync(method);

            Assert.True(id > 0);
            var fromDb = await context.PaymentMethods.FindAsync(id);
            Assert.NotNull(fromDb);
        }

        [Fact]
        public async Task GetPaymentMethodByIdAsync_ReturnsEntity()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var method = CreateSamplePaymentMethod(5);
            context.PaymentMethods.Add(method);
            await context.SaveChangesAsync();

            var repo = new PaymentMethodRepository(context);
            var result = await repo.GetPaymentMethodByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal("Credit Card", result!.Name);
        }

        [Fact]
        public async Task GetPaymentMethodByIdAsync_ReturnsNullIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PaymentMethodRepository(context);

            var result = await repo.GetPaymentMethodByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPaymentMethodsAsync_ReturnsAllMethods()
        {
            using var context = new BookAndDockContext(_dbOptions);
            context.PaymentMethods.AddRange(
                CreateSamplePaymentMethod(1),
                CreateSamplePaymentMethod(2)
            );
            await context.SaveChangesAsync();

            var repo = new PaymentMethodRepository(context);
            var results = (await repo.GetAllPaymentMethodsAsync()).ToList();

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public async Task UpdatePaymentMethodAsync_UpdatesEntity()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var method = CreateSamplePaymentMethod(10);
            context.PaymentMethods.Add(method);
            await context.SaveChangesAsync();

            method.Name = "Updated Name";

            var repo = new PaymentMethodRepository(context);
            await repo.UpdatePaymentMethodAsync(method);

            var updated = await context.PaymentMethods.FindAsync(10);
            Assert.Equal("Updated Name", updated!.Name);
        }

        [Fact]
        public async Task DeletePaymentMethodAsync_RemovesIfExists()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var method = CreateSamplePaymentMethod(20);
            context.PaymentMethods.Add(method);
            await context.SaveChangesAsync();

            var repo = new PaymentMethodRepository(context);
            await repo.DeletePaymentMethodAsync(20);

            var deleted = await context.PaymentMethods.FindAsync(20);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeletePaymentMethodAsync_NoOpIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new PaymentMethodRepository(context);

            var ex = await Record.ExceptionAsync(() => repo.DeletePaymentMethodAsync(999));
            Assert.Null(ex); // should not throw
        }
    }
}
