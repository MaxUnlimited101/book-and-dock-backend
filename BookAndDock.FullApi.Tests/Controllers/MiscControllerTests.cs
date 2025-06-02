using Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class MiscControllerTests
    {
        private readonly MiscController _controller;

        public MiscControllerTests()
        {
            _controller = new MiscController();
        }

        [Fact]
        public void HealthCheck_ReturnsOkWithStatus()
        {
            // Act
            var result = _controller.HealthCheck();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            Assert.NotNull(value);

            // Check anonymous object using dynamic or pattern matching
            var dict = Assert.IsType<Dictionary<string, string>>(value.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(value)?.ToString()));

            Assert.True(dict.ContainsKey("status"));
            Assert.Equal("healthy", dict["status"]);
        }
    }
}
