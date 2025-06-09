using Backend.Data;
using Backend.DTO.Image;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ImageService : IImageService
    {
        private readonly BookAndDockContext _context;
        private readonly S3Service _s3Service;

        public ImageService(BookAndDockContext context, S3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<Image> UploadImageAsync(UploadImageDto dto)
        {
            var url = await _s3Service.UploadFileAsync(dto.File);

            var image = new Image
            {
                Url = url,
                CreatedBy = dto.CreatedBy,
                GuideId = dto.GuideId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<Stream?> GetImageStreamByIdAsync(int id)
        {
            var image = await GetByIdAsync(id);
            if (image == null || string.IsNullOrWhiteSpace(image.Url))
                return null;

            var key = ExtractS3KeyFromUrl(image.Url);
            return await _s3Service.GetFileAsync(key);
        }

        private string ExtractS3KeyFromUrl(string url)
        {
            var uri = new Uri(url);
            return uri.AbsolutePath.TrimStart('/');
        }

        public async Task<bool> DeleteImageByIdAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return false;

            // Extract the S3 key from the image URL
            var s3Key = new Uri(image.Url).AbsolutePath.TrimStart('/');

            // Delete file from S3
            await _s3Service.DeleteFileAsync(s3Key);

            // Remove DB record
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
