namespace Backend.DTO;

public record ImageDto(
    int Id,
    int? CreatorId,
    int? GuideId,
    DateTime? CreatedAt
)
{
    public static ImageDto FromModel(Models.Image image)
    {
        return new ImageDto(
            image.Id,
            image.CreatedBy,
            image.GuideId,
            image.CreatedOn
        );
    }
}