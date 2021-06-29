using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class EditPasswordDTO
    {
        [Required]
        [MaxLength(255)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(255)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class NewPasswordDTO
    {
        [Required]
        [MaxLength(255)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}