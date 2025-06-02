using Backend.Models;

namespace Backend.DTO
{
    public record PortDto(
        int Id,
        string Name,
        string Description,
        bool IsApproved,
        int OwnerId,
        DateTime CreatedOn
    );

    public static class PortModelExtension
    {
        public static PortDto ToDto(this Port port)
        {
            return new PortDto(
                port.Id,
                port.Name,
                port.Description!,
                port.IsApproved,
                port.OwnerId,
                port.CreatedOn!.Value
            );
        }
    }
}
