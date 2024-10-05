using Asp_ImtahanProject_ChatApp.UI.Dtos;

namespace Asp_ImtahanProject_ChatApp.UI.Services
{
    public interface IPhotoService
    {
        Task<string> UploadImageAsync(PhotoCreationDto dto);
    }
}
