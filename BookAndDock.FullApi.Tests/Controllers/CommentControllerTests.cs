using System.Collections.Generic;
using System.Linq;
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
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> _commentServiceMock;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _controller = new CommentController(_commentServiceMock.Object);
        }

        [Fact]
        public void GetAllComments_ReturnsOkWithList()
        {
            var comments = new List<Comment> { new Comment { Id = 1 }, new Comment { Id = 2 } };
            _commentServiceMock.Setup(s => s.GetAllComments()).Returns(comments);

            var result = _controller.GetAllComments();

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Comment>>(ok.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public void GetCommentById_ExistingId_ReturnsOk()
        {
            var comment = new Comment { Id = 1 };
            _commentServiceMock.Setup(s => s.GetCommentById(1)).Returns(comment);

            var result = _controller.GetCommentById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Comment>(ok.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public void GetCommentById_NotFound_ReturnsNotFound()
        {
            _commentServiceMock.Setup(s => s.GetCommentById(99)).Returns((Comment)null);

            var result = _controller.GetCommentById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddComment_Valid_ReturnsCreated()
        {
            var dto = new CommentDto(0, 1, 1, "Nice", null);

            var result = _controller.AddComment(dto);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void AddComment_Invalid_ReturnsBadRequest()
        {
            var dto = new CommentDto(0, 1, 1, "Bad", null);
            _commentServiceMock.Setup(s => s.AddComment(dto)).Throws(new ModelInvalidException("Invalid"));

            var result = _controller.AddComment(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid", bad.Value);
        }

        [Fact]
        public void UpdateComment_Valid_ReturnsOk()
        {
            var dto = new CommentDto(1, 1, 1, "Updated", null);

            var result = _controller.UpdateComment(dto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void UpdateComment_Invalid_ReturnsBadRequest()
        {
            var dto = new CommentDto(1, 1, 1, "Updated", null);
            _commentServiceMock.Setup(s => s.UpdateComment(dto)).Throws(new ModelInvalidException("Update failed"));

            var result = _controller.UpdateComment(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Update failed", bad.Value);
        }

        [Fact]
        public void DeleteComment_Valid_ReturnsOk()
        {
            var result = _controller.DeleteComment(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteComment_Invalid_ReturnsNotFound()
        {
            _commentServiceMock.Setup(s => s.DeleteComment(1)).Throws(new ModelInvalidException("Not found"));

            var result = _controller.DeleteComment(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Not found", notFound.Value);
        }

        [Fact]
        public void GetCommentsByGuideId_Valid_ReturnsOk()
        {
            var comments = new List<Comment> { new Comment { Id = 1 }, new Comment { Id = 2 } };
            _commentServiceMock.Setup(s => s.GetCommentsByGuideId(1)).Returns(comments);

            var result = _controller.GetCommentsByGuideId(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Comment>>(ok.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public void GetCommentsByGuideId_Invalid_ReturnsBadRequest()
        {
            _commentServiceMock.Setup(s => s.GetCommentsByGuideId(1)).Throws(new ModelInvalidException("Invalid"));

            var result = _controller.GetCommentsByGuideId(1);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid", bad.Value);
        }
    }
}
