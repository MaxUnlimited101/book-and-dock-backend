using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Interfaces;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public async Task<ActionResult<PaymentMethodDTO>> GetAll()
        {
            var pms = await _paymentMethodService.GetAllPaymentMethodsAsync();
            return Ok(pms.Select(PaymentMethodDTO.FromModel));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodDTO>> GetById(int id)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            if (paymentMethod == null)
                return NotFound();
            return Ok(PaymentMethodDTO.FromModel(paymentMethod));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDTO dto)
        {
            try
            {
                int newId = await _paymentMethodService.CreatePaymentMethodAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newId });
            }
            catch (ModelInvalidException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethodDTO dto)
        {
            try
            {
                
                await _paymentMethodService.UpdatePaymentMethodAsync(id, dto);
                return NoContent();
            }
            catch (ModelInvalidException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _paymentMethodService.DeletePaymentMethodAsync(id);
                return NoContent();
            }
            catch (ModelInvalidException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}