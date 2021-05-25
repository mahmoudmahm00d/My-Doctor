using FinalProject.Filters;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(8)]
        [StrongPassword]
        public string UserPassword { get; set; }

        [Required]
        [Compare("UserPassword")]
        public string ConfirmPassword { get; set; }
    }
}