using FinalProject.Filters;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(8)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(8)]
        [StrongPassword]
        public string UserPassword { get; set; }

        [Required]
        [Compare("UserPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeEmailDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(8)]
        [StrongPassword]
        public string UserPassword { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}