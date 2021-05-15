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

        [Required(false)]
        public string Jop { get; set; }

        [Range(0,2)]
        [Required(false)]
        public Genders Gender { get; set; }

        [Required]
        public string UserPhone { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string UserPassword { get; set; }

        [Required]
        [Compare("UserPassword")]
        public string ConfirmPassword { get; set; }
    }
}