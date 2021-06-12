using FinalProject.Filters;
using FinalProject.Models;
using System;
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

        public string Jop { get; set; }

        [Range(0,(int)1)]
        public Genders Gender { get; set; }

        [Min22YearsIfADocotor]
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        [Required]
        [PhoneNumber]
        public string UserPhone { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [MinLength(8)]
        [StrongPassword]
        public string UserPassword { get; set; }

        [Required]
        [Compare("UserPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        public byte UserTypeId { get; set; }
    }
}