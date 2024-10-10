
using Asp_ImtahanProject_ChatApp.UI.Models;

namespace Asp_ImtahanProject_ChatApp.UI.Services
{
    public interface IPhotoService
    {
        Task<string> UploadImageAsync(PhotoCreationModel dto);
    }
}
