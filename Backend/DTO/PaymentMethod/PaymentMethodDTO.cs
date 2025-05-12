using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTO;

public record PaymentMethodDTO(
    [Range(0, int.MaxValue)] int Id,

    [Required] string Name,

    DateTime? CreatedAt
)
{
    public PaymentMethodDTO WithId(int id) => this with {Id = id};
    
    public static PaymentMethodDTO FromModel(PaymentMethod paymentMethod)
    {
        return new PaymentMethodDTO(
            paymentMethod.Id,
            paymentMethod.Name,
            paymentMethod.CreatedOn
        );
    }
}