using FinalProject.DTOs;
using FinalProject.Models;
using System.Collections.Generic;

namespace FinalProject.ViewModels
{
    public class SignUpDoctorViewModel
    {
        public SignUpDoctor Doctor { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
    }
}