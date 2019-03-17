using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(60,MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}