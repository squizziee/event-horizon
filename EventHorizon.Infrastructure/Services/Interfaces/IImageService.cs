using Microsoft.AspNetCore.Http;

namespace EventHorizon.Infrastructure.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile image);
        Task DeleteImage(string imageUrl);
    }
}
