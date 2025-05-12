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
    public class PaymentMethodServiceTests
    {
        private readonly Mock<IPaymentMethodRepository> _repoMock;
        private readonly PaymentMethodService _svc;

        public PaymentMethodServiceTests()
        {
            _repoMock = new Mock<IPaymentMethodRepository>();
            _svc = new PaymentMethodService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllPaymentMethodsAsync_ReturnsList()
        {
            var list = new List<PaymentMethod> { new PaymentMethod { Id = 1 }, new PaymentMethod { Id = 2 } };
            _repoMock.Setup(r => r.GetAllPaymentMethodsAsync()).ReturnsAsync(list);
            var result = await _svc.GetAllPaymentMethodsAsync();
            Assert.Equal(list, result);
        }

        [Fact]
        public async Task GetPaymentMethodByIdAsync_Exists_ReturnsItem()
        {
            var item = new PaymentMethod { Id = 3 };
            _repoMock.Setup(r => r.GetPaymentMethodByIdAsync(3)).ReturnsAsync(item);
            var result = await _svc.GetPaymentMethodByIdAsync(3);
            Assert.Equal(item, result);
        }

        [Fact]
        public async Task CreatePaymentMethodAsync_Valid_ReturnsId()
        {
            var dto = new PaymentMethodDTO(0, "Visa", DateTime.UtcNow);
            _repoMock.Setup(r => r.CreatePaymentMethodAsync(It.IsAny<PaymentMethod>())).ReturnsAsync(99);
            var result = await _svc.CreatePaymentMethodAsync(dto);
            Assert.Equal(99, result);
        }

        [Fact]
        public async Task UpdatePaymentMethodAsync_Existing_UpdatesSuccessfully()
        {
            var existing = new PaymentMethod { Id = 5, Name = "Old" };
            var updated = new PaymentMethodDTO(5, "NewName", DateTime.UtcNow);
            _repoMock.Setup(r => r.GetPaymentMethodByIdAsync(5)).ReturnsAsync(existing);

            await _svc.UpdatePaymentMethodAsync(5, updated);

            _repoMock.Verify(r => r.UpdatePaymentMethodAsync(
                It.Is<PaymentMethod>(p => p.Name == updated.Name && p.CreatedOn == updated.CreatedAt)
            ), Times.Once);
        }

        [Fact]
        public async Task UpdatePaymentMethodAsync_NotFound_Throws()
        {
            var updated = new PaymentMethodDTO(5, "NewName", DateTime.UtcNow);
            _repoMock.Setup(r => r.GetPaymentMethodByIdAsync(5)).ReturnsAsync((PaymentMethod?)null);

            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.UpdatePaymentMethodAsync(5, updated));
            Assert.Equal("Payment method not found", ex.Message);
        }

        [Fact]
        public async Task DeletePaymentMethodAsync_Exists_Deletes()
        {
            _repoMock.Setup(r => r.GetPaymentMethodByIdAsync(4)).ReturnsAsync(new PaymentMethod { Id = 4 });
            await _svc.DeletePaymentMethodAsync(4);
            _repoMock.Verify(r => r.DeletePaymentMethodAsync(4), Times.Once);
        }

        [Fact]
        public async Task DeletePaymentMethodAsync_NotFound_Throws()
        {
            _repoMock.Setup(r => r.GetPaymentMethodByIdAsync(6)).ReturnsAsync((PaymentMethod?)null);
            var ex = await Assert.ThrowsAsync<ModelInvalidException>(() => _svc.DeletePaymentMethodAsync(6));
            Assert.Equal("Payment method not found", ex.Message);
        }
    }
}
