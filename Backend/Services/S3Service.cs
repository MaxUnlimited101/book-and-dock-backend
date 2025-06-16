using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service()
    {
        _bucketName = Environment.GetEnvironmentVariable("AWS__BucketName")
            ?? throw new Exception("AWS__BucketName not set");

        _s3Client = new AmazonS3Client(
            Environment.GetEnvironmentVariable("AWS__AccessKey"),
            Environment.GetEnvironmentVariable("AWS__SecretKey"),
            Amazon.RegionEndpoint.GetBySystemName(
                Environment.GetEnvironmentVariable("AWS__Region")
                    ?? throw new Exception("AWS__Region not set")
            )
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

    public async Task<(Stream? Stream, string? ContentType)> GetFileAsync(string key)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            return (response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"S3 error: {e.Message}");
            return (null, null);
        }
    }

    public async Task DeleteFileAsync(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}
