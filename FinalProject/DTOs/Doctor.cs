using FinalProject.Models;
using System.Collections.Generic;

namespace FinalProject.DTOs
{
    public class Doctor
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string FatherName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.IsNullOrEmpty(FatherName)
                  ? $"{FirstName} {LastName}"
                  : $"{FirstName} {FatherName} {LastName}";
            }
        }

        public Genders Gender { get; set; }

        public IEnumerable<Certifcate> Certificates { get; set; }
    }
}