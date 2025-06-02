using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class NotificationControllerTests
    {
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly NotificationController _controller;

        public NotificationControllerTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _controller = new NotificationController(_notificationServiceMock.Object);
        }

        [Fact]
        public async Task GetNotifications_ReturnsList()
        {
            var notifications = new List<Notification> { new Notification { Id = 1, Message = "Hello", CreatedBy = 1 } };
            _notificationServiceMock.Setup(s => s.GetNotificationsAsync()).ReturnsAsync(notifications);

            var result = await _controller.GetNotifications();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<NotificationDTO>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task GetNotificationById_WhenExists_ReturnsNotification()
        {
            var notif = new Notification { Id = 1, Message = "Hi", CreatedBy = 1 };
            _notificationServiceMock.Setup(s => s.GetNotificationByIdAsync(1)).ReturnsAsync(notif);

            var result = await _controller.GetNotificationById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<NotificationDTO>(okResult.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public async Task GetNotificationById_WhenNotFound_ReturnsNotFound()
        {
            _notificationServiceMock.Setup(s => s.GetNotificationByIdAsync(1)).ReturnsAsync((Notification)null);

            var result = await _controller.GetNotificationById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateNotification_Valid_ReturnsCreatedAtAction()
        {
            var dto = new NotificationDTO(1, 1, "message", null);
            _notificationServiceMock.Setup(s => s.CreateNotificationAsync(dto)).ReturnsAsync(1);

            var result = await _controller.CreateNotification(dto);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task CreateNotification_Invalid_ReturnsBadRequest()
        {
            var dto = new NotificationDTO(1, 1, "message", null);
            _notificationServiceMock.Setup(s => s.CreateNotificationAsync(dto)).ThrowsAsync(new ModelInvalidException("error"));

            var result = await _controller.CreateNotification(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("error", bad.Value);
        }

        [Fact]
        public async Task UpdateNotification_Valid_ReturnsNoContent()
        {
            var dto = new NotificationDTO(1, 1, "updated", null);

            var result = await _controller.UpdateNotification(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateNotification_Invalid_ReturnsBadRequest()
        {
            var dto = new NotificationDTO(1, 1, "update", null);
            _notificationServiceMock.Setup(s => s.UpdateNotificationAsync(dto)).ThrowsAsync(new ModelInvalidException("invalid"));

            var result = await _controller.UpdateNotification(1, dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("invalid", bad.Value);
        }

        [Fact]
        public async Task DeleteNotification_Valid_ReturnsNoContent()
        {
            var result = await _controller.DeleteNotification(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteNotification_Invalid_ReturnsBadRequest()
        {
            _notificationServiceMock.Setup(s => s.DeleteNotificationAsync(1)).ThrowsAsync(new ModelInvalidException("fail"));

            var result = await _controller.DeleteNotification(1);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("fail", bad.Value);
        }
    }
}
