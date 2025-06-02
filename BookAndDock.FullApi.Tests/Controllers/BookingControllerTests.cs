using System;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DTO;
using Backend.Services;
using Backend.Models;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Backend.Tests.Controllers
{
    public class BookingControllerTests
    {
        private readonly AuthenticationService _authService;
        private readonly RegistrationService _regService;
        private readonly AuthController _controller;

        public BookingControllerTests()
        {
            var options = new DbContextOptionsBuilder<BookAndDockContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new BookAndDockContext(options);

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_which_is_long_enough_to_pass");
            configMock.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            configMock.Setup(c => c["Jwt:Audience"]).Returns("test-audience");

            var jwtService = new JwtService(configMock.Object);
            _authService = new AuthenticationService(context, jwtService);
            _regService = new RegistrationService(context);
            _controller = new AuthController(_regService, _authService);
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsCreatedUser_Booking()
        {
            var request = new RegisterRequestDto { Name = "John", Surname = "Doe", Email = "john@example.com", Password = "Secret123!" };
            var result = await _controller.Register(request);
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value?.ToString();
            Assert.Contains("john@example.com", response);
        }

        [Fact]
        public async Task Register_ServiceThrows_ReturnsBadRequest_Booking()
        {
            var request = new RegisterRequestDto { Name = "Jane", Surname = "Smith", Email = "john@example.com", Password = "AnotherPass123!" };
            await _controller.Register(request);
            var result = await _controller.Register(request);
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var response = bad.Value?.ToString();
            Assert.Contains("Email is already registered.", response);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken_Booking()
        {
            var request = new RegisterRequestDto { Name = "Tom", Surname = "Thumb", Email = "tom@example.com", Password = "TomSecret1!" };
            await _controller.Register(request);
            var login = new LoginRequestDto { Email = request.Email, Password = request.Password };
            var result = await _controller.Login(login);
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value?.ToString()?.ToLower();
            Assert.Contains("token", response);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized_Booking()
        {
            var login = new LoginRequestDto { Email = "notfound@example.com", Password = "bad" };
            var result = await _controller.Login(login);
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = unauthorized.Value?.ToString();
            Assert.Contains("Invalid email or password.", response);
        }
    }
}
