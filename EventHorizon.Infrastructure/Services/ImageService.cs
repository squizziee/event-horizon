using EventHorizon.Contracts.Exceptions;
using EventHorizon.Infrastructure.Helpers;
using EventHorizon.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EventHorizon.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageUploadOptions _imageUploadOptions;
        private readonly IWebHostEnvironment _environment;
        public ImageService(
            IOptions<ImageUploadOptions> imageUploadOptions,
            IWebHostEnvironment webHostEnvironment) { 
            _imageUploadOptions = imageUploadOptions.Value;
            _environment = webHostEnvironment;
        }

        // return format: https://localhost:0000/path/to/folder/image.jpg
        public async Task<string> UploadImage(IFormFile image)
        {
            var splitted = image.FileName.Split('.');

            if (splitted.Length < 2)
            {
                throw new UnsupportedExtensionException($"Provided file {image.FileName} has no extension");
            }

            var extension = splitted.Last();

            if (!_imageUploadOptions.SupportedExtensions.Contains(extension))
            {
                throw new UnsupportedExtensionException($"Provided file extesion .{extension} is not supported");
            }

            var uploadFileName = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(
                _environment.WebRootPath,
                _imageUploadOptions.Url,
                uploadFileName + $".{extension}"
            );

            using var file = File.Create( uploadPath );
            await image.CopyToAsync(file);

            var accessiblePath =
                _imageUploadOptions.AccessibleUrl + "/" +
                _imageUploadOptions.Url + "/" +
                uploadFileName + $".{extension}";

            return accessiblePath;
        }

        public Task DeleteImage(string accesibleImageUrl)
        {
            // assuming address format is https://localhost:0000/path/to/folder/image.jpg
            // parts are going to be:
            //   - "https:"    [0]
            //   - ""          [1]
            //   - "path"      [2]
            //   - "to"        [3]
            //   - "folder"    [4]
            //   - "image.jpg" [5]
            var pathParts = accesibleImageUrl.Split("/");

            var relativePath =  string.Join("/", pathParts.Skip(2).ToArray());

            if (File.Exists(relativePath))
            {
                File.Delete(relativePath);
                return Task.CompletedTask;
            }

            throw new ResourceNotFoundException($"Can't delete non-existent image: {accesibleImageUrl}");
        }
    }
}
