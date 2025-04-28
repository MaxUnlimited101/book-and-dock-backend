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
    public class GuideServiceTests
    {
        private readonly Mock<IGuideRepository> _guideRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly GuideService _svc;

        public GuideServiceTests()
        {
            _guideRepoMock = new Mock<IGuideRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _svc = new GuideService(
                _guideRepoMock.Object,
                _userRepoMock.Object
            );
        }

        [Fact]
        public void CreateGuide_UserNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new GuideDto(0, "Title", "Content", 1, DateTime.UtcNow, true);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns((User?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.CreateGuide(dto));
            Assert.Equal("User not found", ex.Message);
            _guideRepoMock.Verify(r => r.Add(It.IsAny<Guide>()), Times.Never);
        }

        [Fact]
        public void CreateGuide_Valid_CallsRepositoryAddWithMappedGuide()
        {
            // Arrange
            var dto = new GuideDto(0, "Title", "Content", 1, null, true);
            var user = new User { Id = dto.CreatedBy, Name = "A", Surname = "B", Email = "a@b.com", Password = "p" };

            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns(user);

            // Act
            _svc.CreateGuide(dto);

            // Assert
            _guideRepoMock.Verify(r => r.Add(It.Is<Guide>(g =>
                g.Title == dto.Title &&
                g.Content == dto.Content &&
                g.CreatedBy == dto.CreatedBy &&
                g.CreatedByNavigation == user &&
                g.IsApproved == dto.IsApproved &&
                g.CreatedOn.HasValue
            )), Times.Once);
        }

        [Fact]
        public void DeleteGuide_NotFound_ThrowsModelInvalidException()
        {
            // Arrange
            _guideRepoMock.Setup(r => r.GetGuideById(5)).Returns((Guide?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.DeleteGuide(5));
            Assert.Equal("Guide not found", ex.Message);
        }

        [Fact]
        public void DeleteGuide_Exists_CallsRepositoryDelete()
        {
            // Arrange
            _guideRepoMock.Setup(r => r.GetGuideById(6)).Returns(new Guide { Id = 6 });

            // Act
            _svc.DeleteGuide(6);

            // Assert
            _guideRepoMock.Verify(r => r.Delete(6), Times.Once);
        }

        [Fact]
        public void GetAllGuides_MapsEntitiesToDtos()
        {
            // Arrange
            var list = new List<Guide>
            {
                new Guide { Id = 1, Title = "T1", Content = "C1", CreatedBy = 1, CreatedOn = DateTime.UtcNow, IsApproved = true },
                new Guide { Id = 2, Title = "T2", Content = "C2", CreatedBy = 2, CreatedOn = DateTime.UtcNow, IsApproved = false }
            };
            _guideRepoMock.Setup(r => r.GetAll()).Returns(list);

            // Act
            var result = _svc.GetAllGuides().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(list[0].Id, result[0].Id);
            Assert.Equal(list[1].Id, result[1].Id);
        }

        [Fact]
        public void GetGuideById_NotFound_ReturnsNull()
        {
            // Arrange
            _guideRepoMock.Setup(r => r.GetGuideById(7)).Returns((Guide?)null);

            // Act
            var result = _svc.GetGuideById(7);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetGuideById_Exists_ReturnsMappedDto()
        {
            // Arrange
            var guide = new Guide { Id = 8, Title = "T8", Content = "C8", CreatedBy = 3, CreatedOn = DateTime.UtcNow, IsApproved = true };
            _guideRepoMock.Setup(r => r.GetGuideById(8)).Returns(guide);

            // Act
            var result = _svc.GetGuideById(8);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(guide.Id, result!.Id);
            Assert.Equal(guide.Title, result.Title);
        }

        [Fact]
        public void UpdateGuide_GuideNotFound_ThrowsModelInvalidException()
        {
            // Arrange
            var dto = new GuideDto(9, "UpdatedTitle", "UpdatedContent", 2, DateTime.UtcNow, true);
            _guideRepoMock.Setup(r => r.GetGuideById(dto.Id)).Returns((Guide?)null);

            // Act & Assert
            var ex = Assert.Throws<ModelInvalidException>(() => _svc.UpdateGuide(dto));
            Assert.Equal("Guide not found", ex.Message);
        }

        [Fact]
        public void UpdateGuide_Valid_UpdatesEntityAndCallsRepositoryUpdate()
        {
            // Arrange
            var dto = new GuideDto(10, "UpdatedTitle", "UpdatedContent", 2, DateTime.UtcNow, false);
            var existing = new Guide { Id = dto.Id, Title = "Old", Content = "Old", CreatedBy = 1, IsApproved = true };
            var user = new User { Id = dto.CreatedBy, Name = "A", Surname = "B", Email = "e@e.com", Password = "p" };

            _guideRepoMock.Setup(r => r.GetGuideById(dto.Id)).Returns(existing);
            _userRepoMock.Setup(r => r.GetUserById(dto.CreatedBy)).Returns(user);

            // Act
            _svc.UpdateGuide(dto);

            // Assert
            _guideRepoMock.Verify(r => r.Update(It.Is<Guide>(g =>
                g.Id == dto.Id &&
                g.Title == dto.Title &&
                g.Content == dto.Content &&
                g.CreatedBy == dto.CreatedBy &&
                g.CreatedByNavigation == user &&
                g.IsApproved == dto.IsApproved
            )), Times.Once);
        }
    }
}
