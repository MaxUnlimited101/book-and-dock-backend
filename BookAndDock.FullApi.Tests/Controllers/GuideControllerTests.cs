using System.Collections.Generic;
using System.Linq;
using Backend.Controllers;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class GuideControllerTests
    {
        private readonly Mock<IGuideService> _guideServiceMock;
        private readonly GuideController _controller;

        public GuideControllerTests()
        {
            _guideServiceMock = new Mock<IGuideService>();
            _controller = new GuideController(_guideServiceMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsOkWithGuides()
        {
            var guides = new List<GuideDto> { new GuideDto(1, "Title", "Content", 1, null, true) };
            _guideServiceMock.Setup(s => s.GetAllGuides()).Returns(guides);

            var result = _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<GuideDto>>(ok.Value);
            Assert.Single(returned);
        }

        [Fact]
        public void GetById_ExistingId_ReturnsOk()
        {
            var guide = new GuideDto(1, "Title", "Content", 1, null, true);
            _guideServiceMock.Setup(s => s.GetGuideById(1)).Returns(guide);

            var result = _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<GuideDto>(ok.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public void GetById_NotFound_ReturnsNotFound()
        {
            _guideServiceMock.Setup(s => s.GetGuideById(99)).Returns((GuideDto)null);

            var result = _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Valid_ReturnsCreated()
        {
            var guide = new GuideDto(0, "New", "Content", 1, null, true);

            var result = _controller.Create(guide);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void Create_Invalid_ReturnsBadRequest()
        {
            var guide = new GuideDto(0, "Bad", "Content", 1, null, true);
            _guideServiceMock.Setup(s => s.CreateGuide(guide)).Throws(new ModelInvalidException("Invalid"));

            var result = _controller.Create(guide);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid", bad.Value);
        }

        [Fact]
        public void Update_Valid_ReturnsOk()
        {
            var guide = new GuideDto(1, "Updated", "Content", 1, null, true);

            _guideServiceMock.Setup(s => s.UpdateGuide(1, guide)).Verifiable();

            var result = _controller.Update(1, guide);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Update_Invalid_ReturnsBadRequest()
        {
            var guide = new GuideDto(1, "Updated", "Content", 1, null, true);

            _guideServiceMock.Setup(s => s.UpdateGuide(1, guide))
                .Throws(new ModelInvalidException("Update failed"));

            var result = _controller.Update(1, guide);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Update failed", bad.Value);
        }

        [Fact]
        public void Delete_Valid_ReturnsNoContent()
        {
            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
