using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentMethodController : ControllerBase
    {
        // private readonly IPaymentMethodService _paymentMethodService;

        // public PaymentMethodController(IPaymentMethodService paymentMethodService)
        // {
        //     _paymentMethodService = paymentMethodService;
        // }

        // [HttpGet]
        // public IActionResult GetAll()
        // {
        //     return Ok(PaymentMethods);
        // }

        // [HttpGet("{id}")]
        // public IActionResult GetById(int id)
        // {
        //     if (id < 0 || id >= PaymentMethods.Count)
        //         return NotFound();

        //     return Ok(PaymentMethods[id]);
        // }

        // [HttpPost]
        // public IActionResult Create([FromBody] string paymentMethod)
        // {
        //     if (string.IsNullOrWhiteSpace(paymentMethod))
        //         return BadRequest("Payment method cannot be empty.");

        //     PaymentMethods.Add(paymentMethod);
        //     return CreatedAtAction(nameof(GetById), new { id = PaymentMethods.Count - 1 }, paymentMethod);
        // }

        // [HttpPut("{id}")]
        // public IActionResult Update(int id, [FromBody] string updatedPaymentMethod)
        // {
        //     if (id < 0 || id >= PaymentMethods.Count)
        //         return NotFound();

        //     if (string.IsNullOrWhiteSpace(updatedPaymentMethod))
        //         return BadRequest("Payment method cannot be empty.");

        //     PaymentMethods[id] = updatedPaymentMethod;
        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        //     if (id < 0 || id >= PaymentMethods.Count)
        //         return NotFound();

        //     PaymentMethods.RemoveAt(id);
        //     return NoContent();
        // }
    }
}