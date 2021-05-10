using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class SignUpUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        public string FatherName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string Jop { get; set; }

        [Range(0,2)]
        public Genders Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}