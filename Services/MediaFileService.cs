using BlogPortal.Models;
using BlogPortal.Repositories;
using BlogPortal.Shared;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace BlogPortal.Services
{
    public class FileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly MediaFileRepository _fileRepo;

        public FileService(IOptions<CloudinarySettings> config, MediaFileRepository fileRepo)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
            _fileRepo = fileRepo;
        }

        public async Task<ApiResponse<object>> AddFileAsync(IFormFile formFile, int userId)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return new ApiResponse<object>(false, "File not provided");
            }

            await using var stream = formFile.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(formFile.FileName, stream),
                Folder = "DEFAULT"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            var extension = Path.GetExtension(formFile.FileName)?.TrimStart('.').ToLower();
            var format = string.IsNullOrEmpty(extension) ? "unknown" : extension;
            Console.WriteLine("userId " + userId);
            var file = new MediaFile
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
                Format = format,
                Folder = "DEFAULT",
                UserId = userId,
            };

            await _fileRepo.AddFileAsync(file);

            return new ApiResponse<object>(true, "File uploaded successfully", new
            {
                file.Id,
                file.Url,
                file.PublicId,
                file.Format,
                file.UserId
            });
        }
    }
}
