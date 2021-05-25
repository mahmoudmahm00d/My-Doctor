using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }

        [StringLength(50)]
        public string Jop { get; set; }

        public UserType UserType { get; set; }
        public byte UserTypeId { get; set; }

        [Required]
        [StringLength(15)]
        public string UserPhone { get; set; }

        [Required]
        [StringLength(255)]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string UserPassword { get; set; }

        [StringLength(6)]
        public string VerCode { get; set; }

        public bool Locked { get; set; }

        public IEnumerable<Certifcate> Certifcates { get; set; }
    }

    public enum Genders
    {
        Male = 0, Female = 1
    }
}