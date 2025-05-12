using System;
using System.Collections.Generic;
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
    public class NotificationServiceTests
    {
        private readonly Mock<INotificationRepository> _notifRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly NotificationService _svc;

        public NotificationServiceTests()
        {
            _notifRepoMock = new Mock<INotificationRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _svc = new NotificationService(_notifRepoMock.Object, _userRepoMock.Object);
        }

        [Fact]
        public async Task GetNotificationsAsync_ReturnsAll()
        {
            var list = new List<Notification> { new Notification { Id = 1 }, new Notification { Id = 2 } };
            _notifRepoMock.Setup(r => r.GetNotificationsAsync()).ReturnsAsync(list);
            var result = await _svc.GetNotificationsAsync();
            Assert.Equal(list, result);
        }

        [Fact]
        public async Task GetNotificationByIdAsync_Exists_ReturnsNotification()
        {
            var notif = new Notification { Id = 5 };
            _notifRepoMock.Setup(r => r.GetNotificationByIdAsync(5)).ReturnsAsync(notif);
            var result = await _svc.GetNotificationByIdAsync(5);
            Assert.Equal(notif, result);
        }

        [Fact]
        public async Task GetNotificationByIdAsync_NotFound_Throws()
        {
            _notifRepoMock.Setup(r => r.GetNotificationByIdAsync(99)).ReturnsAsync((Notification?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.GetNotificationByIdAsync(99));
            Assert.Equal("Notification not found", ex.Message);
        }

        [Fact]
        public async Task CreateNotificationAsync_Valid_ReturnsId()
        {
            var dto = new NotificationDTO(0, 1, "Hello", null);
            var notif = new Notification { Id = 1, Message = "Hello", CreatedBy = 1 };
            _userRepoMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(new User { Id = 1 });
            _notifRepoMock.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>())).ReturnsAsync(101);

            var result = await _svc.CreateNotificationAsync(dto);
            Assert.Equal(101, result);
        }

        [Fact]
        public async Task CreateNotificationAsync_NullUserId_Throws()
        {
            var dto = new NotificationDTO(0, null, "Hello", null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.CreateNotificationAsync(dto));
            Assert.Equal("User id is required", ex.Message);
        }

        [Fact]
        public async Task UpdateNotificationAsync_NotificationExists_Updates()
        {
            var dto = new NotificationDTO(1, 1, "Updated", null);
            var notif = new Notification { Id = 1, CreatedBy = 1, Message = "Old" };
            _notifRepoMock.Setup(r => r.GetNotificationByIdAsync(1)).ReturnsAsync(notif);

            await _svc.UpdateNotificationAsync(dto);

            _notifRepoMock.Verify(r => r.UpdateNotificationAsync(It.Is<Notification>(n => n.Message == "Updated" && n.CreatedBy == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateNotificationAsync_NotFound_Throws()
        {
            var dto = new NotificationDTO(9, 1, "Test", null);
            _notifRepoMock.Setup(r => r.GetNotificationByIdAsync(9)).ReturnsAsync((Notification?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdateNotificationAsync(dto));
            Assert.Equal("Notification not found", ex.Message);
        }

        [Fact]
        public async Task DeleteNotificationAsync_Exists_Deletes()
        {
            _notifRepoMock.Setup(r => r.GetNotificationByIdAsync(4)).ReturnsAsync(new Notification { Id = 4 });
            await _svc.DeleteNotificationAsync(4);
            _notifRepoMock.Verify(r => r.DeleteNotificationAsync(4), Times.Once);
        }

        [Fact]
        public async Task DeleteNotificationAsync_NotFound_Throws()
        {
            _notifRepoMock.Setup(r => r.GetNotificationByIdAsync(6)).ReturnsAsync((Notification?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.DeleteNotificationAsync(6));
            Assert.Equal("Notification not found", ex.Message);
        }
    }
}
