namespace Backend.DTO.Port
{
    public record PortDto(
        string Name,
        string Description,
        bool IsApproved
    );
}
