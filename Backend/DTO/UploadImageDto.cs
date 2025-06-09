namespace Backend.DTO.Image
{
    public record UploadImageDto(
    IFormFile File,
    int? GuideId,
    int? CreatedBy
);
}
