using System;
using System.Collections.Generic;
using System.Linq;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Moq;
using Xunit;

namespace Backend.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _commentRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IGuideRepository> _guideRepoMock;
        private readonly CommentService _svc;

        public CommentServiceTests()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _guideRepoMock = new Mock<IGuideRepository>();
            _svc = new CommentService(
                _commentRepoMock.Object,
                _userRepoMock.Object,
                _guideRepoMock.Object
            );
        }

        [Fact]
        public void AddComment_UserNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new CommentDto(Id: 0, CreatedBy: 1, GuideId: 2, Content: "Test content", CreatedOn: null);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns((User?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.AddComment(dto));
            Assert.Equal("User not found", ex.Message);
            _commentRepoMock.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
        }

        [Fact]
        public void AddComment_GuideNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new CommentDto(Id: 0, CreatedBy: 1, GuideId: 2, Content: "Test content", CreatedOn: null);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns(new User { Id = dto.CreatedBy, Name = "A", Surname = "B", Email = "a@b.com", Password = "p" });
            _guideRepoMock.Setup(r => r.GetGuideById(dto.GuideId)).Returns((Guide?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.AddComment(dto));
            Assert.Equal("Guide not found", ex.Message);
            _commentRepoMock.Verify(r => r.Add(It.IsAny<Comment>()), Times.Never);
        }

        [Fact]
        public void AddComment_Valid_CallsRepositoryAddWithMappedComment()
        {
            // Arrange
            var dto = new CommentDto(Id: 0, CreatedBy: 1, GuideId: 2, Content: "Hello world", CreatedOn: null);
            var user = new User { Id = dto.CreatedBy, Name = "A", Surname = "B", Email = "a@b.com", Password = "p" };
            var guide = new Guide { Id = dto.GuideId, Title = "T", Content = "C" };

            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns(user);
            _guideRepoMock.Setup(r => r.GetGuideById(dto.GuideId)).Returns(guide);

            // Act
            _svc.AddComment(dto);

            // Assert
            _commentRepoMock.Verify(r => r.Add(It.Is<Comment>(c =>
                c.CreatedBy == dto.CreatedBy &&
                c.GuideId == dto.GuideId &&
                c.Content == dto.Content &&
                c.CreatedByNavigation == user &&
                c.Guide == guide &&
                c.CreatedOn.HasValue
            )), Times.Once);
        }

        [Fact]
        public void DeleteComment_Exists_DeletesViaRepository()
        {
            // Arrange
            var existing = new Comment { Id = 5 };
            _commentRepoMock.Setup(r => r.GetById(existing.Id)).Returns(existing);

            // Act
            _svc.DeleteComment(existing.Id);

            // Assert
            _commentRepoMock.Verify(r => r.Delete(existing.Id), Times.Once);
        }

        [Fact]
        public void DeleteComment_NotExists_ThrowsModelInvalidException()
        {
            // Arrange
            _commentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Comment?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.DeleteComment(10));
            Assert.Equal("Comment not found", ex.Message);
        }

        [Fact]
        public void GetAllComments_ReturnsAllFromRepository()
        {
            // Arrange
            var list = new List<Comment> { new Comment { Id = 1 }, new Comment { Id = 2 } };
            _commentRepoMock.Setup(r => r.GetAll()).Returns(list);

            // Act
            var result = _svc.GetAllComments();

            // Assert
            Assert.Equal(list, result);
        }

        [Fact]
        public void GetCommentById_Exists_ReturnsComment()
        {
            // Arrange
            var expected = new Comment { Id = 7 };
            _commentRepoMock.Setup(r => r.GetById(expected.Id)).Returns(expected);

            // Act
            var result = _svc.GetCommentById(expected.Id);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCommentById_NotExists_ReturnsNull()
        {
            // Arrange
            _commentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Comment?)null);

            // Act
            var result = _svc.GetCommentById(8);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetCommentsByGuideId_GuideNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            _guideRepoMock.Setup(r => r.GetGuideById(It.IsAny<int>())).Returns((Guide?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.GetCommentsByGuideId(3));
            Assert.Equal("Guide not found", ex.Message);
        }

        [Fact]
        public void GetCommentsByGuideId_Valid_FiltersByGuideId()
        {
            // Arrange
            var guideId = 4;
            var comments = new List<Comment>
            {
                new Comment { Id = 1, GuideId = guideId },
                new Comment { Id = 2, GuideId = 99 }
            };
            _guideRepoMock.Setup(r => r.GetGuideById(guideId)).Returns(new Guide { Id = guideId });
            _commentRepoMock.Setup(r => r.GetAll()).Returns(comments);

            // Act
            var result = _svc.GetCommentsByGuideId(guideId).ToList();

            // Assert
            Assert.Single(result);
            Assert.All(result, c => Assert.Equal(guideId, c.GuideId));
        }

        [Fact]
        public void UpdateComment_NotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new CommentDto(Id: 9, CreatedBy: 1, GuideId: 2, Content: "X", CreatedOn: null);
            _commentRepoMock.Setup(r => r.GetById(dto.Id)).Returns((Comment?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateComment(dto));
            Assert.Equal("Comment not found", ex.Message);
        }

        [Fact]
        public void UpdateComment_UserNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new CommentDto(Id: 10, CreatedBy: 1, GuideId: 2, Content: "X", CreatedOn: null);
            var existing = new Comment { Id = dto.Id };
            _commentRepoMock.Setup(r => r.GetById(dto.Id)).Returns(existing);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns((User?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateComment(dto));
            Assert.Equal("User not found", ex.Message);
        }

        [Fact]
        public void UpdateComment_GuideNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new CommentDto(Id: 11, CreatedBy: 1, GuideId: 2, Content: "X", CreatedOn: null);
            var existing = new Comment { Id = dto.Id };
            _commentRepoMock.Setup(r => r.GetById(dto.Id)).Returns(existing);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns(new User { Id = dto.CreatedBy, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" });
            _guideRepoMock.Setup(r => r.GetGuideById(dto.GuideId)).Returns((Guide?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateComment(dto));
            Assert.Equal("Guide not found", ex.Message);
        }

        [Fact]
        public void UpdateComment_Valid_CallsRepositoryUpdateWithMappedComment()
        {
            // Arrange
            var dto = new CommentDto(Id: 12, CreatedBy: 1, GuideId: 2, Content: "Updated content", CreatedOn: null);
            var existing = new Comment { Id = dto.Id, Content = "Old", CreatedOn = DateTime.UtcNow.AddDays(-1) };
            var user = new User { Id = dto.CreatedBy, Name = "A", Surname = "B", Email = "x@x.com", Password = "p" };
            var guide = new Guide { Id = dto.GuideId, Title = "T", Content = "C" };
            _commentRepoMock.Setup(r => r.GetById(dto.Id)).Returns(existing);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns(user);
            _guideRepoMock.Setup(r => r.GetGuideById(dto.GuideId)).Returns(guide);

            // Act
            _svc.UpdateComment(dto);

            // Assert
            _commentRepoMock.Verify(r => r.Update(It.Is<Comment>(c =>
                c.Id == dto.Id &&
                c.CreatedBy == dto.CreatedBy &&
                c.CreatedByNavigation == user &&
                c.GuideId == dto.GuideId &&
                c.Guide == guide &&
                c.Content == dto.Content
            )), Times.Once);
        }
    }
}
