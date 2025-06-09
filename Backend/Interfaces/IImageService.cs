using Backend.DTO.Image;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IImageService
    {
        Task<Image> UploadImageAsync(UploadImageDto dto);
        Task<Stream?> GetImageStreamByIdAsync(int id);
        Task<bool> DeleteImageByIdAsync(int id);
    }
}
