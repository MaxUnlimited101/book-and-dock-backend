using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTO;

public record NotificationDTO(
    [Range(0, int.MaxValue)]
    int Id,
    
    [Range(0, int.MaxValue)]
    [Required]
    int? CreatedBy,
    
    [Required]
    string Message,
    
    DateTime? CreatedOn
)
{
    public NotificationDTO WithId(int id) => this with { Id = id };
    
    public static NotificationDTO FromModel(Notification model)
    {
        return new NotificationDTO(
            model.Id,
            model.CreatedBy,
            model.Message,
            model.CreatedOn
        );
    }
}