using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Interfaces
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<List<string>> UploadFilesAsync(List<IFormFile> files);
        Task DeleteFileAsync(string fileUrl);
    }
}