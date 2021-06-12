using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class EditEmailDTO
    {
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
    }
}