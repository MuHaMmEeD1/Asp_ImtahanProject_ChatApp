using System.ComponentModel.DataAnnotations;

namespace Asp_ImtahanProject_ChatApp.UI.Models.RegisterModels
{
    public class RegisterModel
    {


        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9 _-]+$", ErrorMessage = "Name can only contain English letters, numbers, spaces, underscores, and hyphens.")]
        public string Name { get; set; }
        

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_]).{8,}$",
    ErrorMessage = "Password must contain at least 8 characters, including at least one uppercase letter, one lowercase letter, one number, and one underscore.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must accept the privacy policy.")]
        public bool Privacy { get; set; }

    }
}
