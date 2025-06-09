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
    public class NotificationRepositoryTests
    {
        private readonly DbContextOptions<BookAndDockContext> _dbOptions;

        public NotificationRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private Notification CreateSampleNotification(int id = 0)
        {
            return new Notification
            {
                Id = id,
                Message = "Test Message",
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task CreateNotificationAsync_AddsToDatabaseAndReturnsId()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new NotificationRepository(context);
            var notification = CreateSampleNotification();

            var id = await repo.CreateNotificationAsync(notification);

            Assert.True(id > 0);
            var fromDb = await context.Notifications.FindAsync(id);
            Assert.NotNull(fromDb);
        }

        [Fact]
        public async Task GetNotificationByIdAsync_ReturnsNotification()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var notification = CreateSampleNotification(5);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var repo = new NotificationRepository(context);
            var result = await repo.GetNotificationByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal("Test Message", result!.Message);
        }

        [Fact]
        public async Task GetNotificationByIdAsync_ReturnsNullIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new NotificationRepository(context);

            var result = await repo.GetNotificationByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetNotificationsAsync_ReturnsAllNotifications()
        {
            using var context = new BookAndDockContext(_dbOptions);
            context.Notifications.AddRange(
                CreateSampleNotification(1),
                CreateSampleNotification(2)
            );
            await context.SaveChangesAsync();

            var repo = new NotificationRepository(context);
            var results = (await repo.GetNotificationsAsync()).ToList();

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public async Task UpdateNotificationAsync_UpdatesEntity()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var notification = CreateSampleNotification(10);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            notification.Message = "Updated Message";

            var repo = new NotificationRepository(context);
            await repo.UpdateNotificationAsync(notification);

            var updated = await context.Notifications.FindAsync(10);
            Assert.Equal("Updated Message", updated!.Message);
        }

        [Fact]
        public async Task DeleteNotificationAsync_RemovesIfExists()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var notification = CreateSampleNotification(20);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var repo = new NotificationRepository(context);
            await repo.DeleteNotificationAsync(20);

            var deleted = await context.Notifications.FindAsync(20);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task DeleteNotificationAsync_NoOpIfNotFound()
        {
            using var context = new BookAndDockContext(_dbOptions);
            var repo = new NotificationRepository(context);

            var ex = await Record.ExceptionAsync(() => repo.DeleteNotificationAsync(1234));
            Assert.Null(ex); // should silently succeed
        }
    }
}
