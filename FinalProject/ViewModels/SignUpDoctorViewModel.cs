using FinalProject.DTOs;
using FinalProject.Models;
using System.Collections.Generic;

namespace FinalProject.ViewModels
{
    public class SignUpDoctorViewModel
    {
        public SignUpDoctorViewModel()
        {
            List<Gender> l = new List<Gender>();
            l.Add(new Gender { Value = (int)Genders.Male, Name = "Male" });
            l.Add(new Gender { Value = (int)Genders.Female, Name = "Female" });
            Gender = l;
        }
        public SignUpDoctor Doctor { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
        public IEnumerable<Gender> Gender { get; set; }
    }
    public class Gender
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}