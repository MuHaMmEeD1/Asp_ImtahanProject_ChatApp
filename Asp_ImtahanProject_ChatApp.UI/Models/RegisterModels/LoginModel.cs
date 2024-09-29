using System.ComponentModel.DataAnnotations;

namespace Asp_ImtahanProject_ChatApp.UI.Models.RegisterModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username Or Email is required.")]        
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
