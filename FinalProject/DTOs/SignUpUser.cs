using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class SignUpUser
    {
        public int UserId { get; set; }
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
        public string UserPhone { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string UserPassword { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}