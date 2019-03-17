using System.ComponentModel.DataAnnotations;

namespace Blog.API.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}