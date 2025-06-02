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
    public class PaymentMethodControllerTests
    {
        private readonly Mock<IPaymentMethodService> _paymentMethodServiceMock;
        private readonly PaymentMethodController _controller;

        public PaymentMethodControllerTests()
        {
            _paymentMethodServiceMock = new Mock<IPaymentMethodService>();
            _controller = new PaymentMethodController(_paymentMethodServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsList()
        {
            var methods = new List<PaymentMethod>
            {
                new PaymentMethod { Id = 1, Name = "Credit Card", CreatedOn = DateTime.UtcNow }
            };
            _paymentMethodServiceMock.Setup(s => s.GetAllPaymentMethodsAsync()).ReturnsAsync(methods);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<PaymentMethodDTO>>(okResult.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task GetById_WhenExists_ReturnsOk()
        {
            var pm = new PaymentMethod { Id = 1, Name = "Credit Card", CreatedOn = DateTime.UtcNow };
            _paymentMethodServiceMock.Setup(s => s.GetPaymentMethodByIdAsync(1)).ReturnsAsync(pm);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<PaymentMethodDTO>(okResult.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public async Task GetById_WhenNotFound_ReturnsNotFound()
        {
            _paymentMethodServiceMock.Setup(s => s.GetPaymentMethodByIdAsync(1)).ReturnsAsync((PaymentMethod)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new PaymentMethodDTO(0, "PayPal", DateTime.UtcNow);
            _paymentMethodServiceMock.Setup(s => s.CreatePaymentMethodAsync(dto)).ReturnsAsync(1);

            var result = await _controller.Create(dto);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task Create_Invalid_ReturnsBadRequest()
        {
            var dto = new PaymentMethodDTO(0, "", DateTime.UtcNow);
            _paymentMethodServiceMock.Setup(s => s.CreatePaymentMethodAsync(dto)).ThrowsAsync(new ModelInvalidException("error"));

            var result = await _controller.Create(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("error", bad.Value);
        }

        [Fact]
        public async Task Update_Valid_ReturnsNoContent()
        {
            var dto = new PaymentMethodDTO(1, "Stripe", DateTime.UtcNow);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_Invalid_ReturnsBadRequest()
        {
            var dto = new PaymentMethodDTO(1, "Stripe", DateTime.UtcNow);
            _paymentMethodServiceMock.Setup(s => s.UpdatePaymentMethodAsync(1, dto)).ThrowsAsync(new ModelInvalidException("fail"));

            var result = await _controller.Update(1, dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("fail", bad.Value);
        }

        [Fact]
        public async Task Delete_Valid_ReturnsNoContent()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_Invalid_ReturnsBadRequest()
        {
            _paymentMethodServiceMock.Setup(s => s.DeletePaymentMethodAsync(1)).ThrowsAsync(new ModelInvalidException("fail"));

            var result = await _controller.Delete(1);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("fail", bad.Value);
        }
    }
}
