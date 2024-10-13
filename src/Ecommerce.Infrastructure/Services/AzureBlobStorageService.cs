using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = configuration["AppSettings:AzureBlobStorageContainerName"];
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = containerClient.GetBlobClient(fileName);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            return blobClient.Uri.ToString();
        }

        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files)
        {
            var urls = new List<string>();
            foreach (var file in files)
            {
                var url = await UploadFileAsync(file);
                urls.Add(url);
            }
            return urls;
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var uri = new Uri(fileUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
