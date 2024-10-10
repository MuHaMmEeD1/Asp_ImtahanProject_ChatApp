
using Asp_ImtahanProject_ChatApp.UI.Models;
using Asp_ImtahanProject_ChatApp.UI.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Asp_ImtahanProject_ChatApp.UI.Services
{
    public class PhotoService : IPhotoService
    {
        private IConfiguration _configuration;
        private ClouddinarySettings _clouddinarySettings;
        private Cloudinary _cloudinary;

        public PhotoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _clouddinarySettings = _configuration.GetSection("CloudinarySettings").Get<ClouddinarySettings>();
            Account account = new Account(_clouddinarySettings.CloudName, _clouddinarySettings.ApiKey, _clouddinarySettings.ApiSecret);
            _cloudinary=new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(PhotoCreationModel dto)
        {
            ImageUploadResult uploadedResult = new ImageUploadResult();

            IFormFile? file = dto.File;
            if(file?.Length > 0)
            {
                using (Stream? stream = file.OpenReadStream())
                {
                    ImageUploadParams uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadedResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadedResult != null)
                    {
                        return uploadedResult.Url.ToString();
                    }
                }
            }
            return "";
        }
    }
}
