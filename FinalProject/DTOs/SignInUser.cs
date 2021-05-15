using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class SignInUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}