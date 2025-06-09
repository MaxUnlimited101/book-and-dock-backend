using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IConfiguration config)
    {
        _bucketName = config["AWS:BucketName"];
        _s3Client = new AmazonS3Client(
            config["AWS:AccessKey"],
            config["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(config["AWS:Region"])
        );
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileKey = $"images/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = fileKey,
            BucketName = _bucketName,
            ContentType = file.ContentType
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{_bucketName}.s3.amazonaws.com/{fileKey}";
    }

    public async Task<Stream?> GetFileAsync(string key)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"S3 error: {e.Message}");
            return null;
        }
    }

    public async Task DeleteFileAsync(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = "book-and-dock-bucket",
            Key = key
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}
