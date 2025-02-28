using AcopioAPIs.Utils;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace AcopioAPIs.Service
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public Cloudinary GetCloudinaryInstance()
        {
            return _cloudinary;
        }
    }
}
