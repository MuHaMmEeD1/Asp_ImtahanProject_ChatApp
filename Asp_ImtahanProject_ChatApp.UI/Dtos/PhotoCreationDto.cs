using Microsoft.AspNetCore.Http;

namespace Asp_ImtahanProject_ChatApp.UI.Dtos
{
    public class PhotoCreationDto
    {
        public IFormFile ?File { get; set; }
    }
}
