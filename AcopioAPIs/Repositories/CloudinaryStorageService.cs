using AcopioAPIs.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AcopioAPIs.Repositories
{
    public class CloudinaryStorageService : IStorageService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryStorageService(CloudinaryService cloudinaryService)
        {
            _cloudinary = cloudinaryService.GetCloudinaryInstance();
        }

        public async Task<string> UploadImageAsync(string nombreCarpeta, Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = $"{nombreCarpeta}/{Guid.NewGuid()}",
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

    }
}
