using System.ComponentModel.DataAnnotations;

namespace Asp_ImtahanProject_ChatApp.UI.Models.HomeModels
{
    public class PostModel
    {
        [Required(ErrorMessage = "Message Cannot Be Empty")]
        public string Message { get; set; }
        public IFormFile? Photo { get; set; }
        public string? VideoUrl { get; set; }
        public string? Tags { get; set; }
        public DateTime PostDate { get; set; }
      
    }
}
