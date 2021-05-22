using FinalProject.DTOs;
using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class UsersManageViewModel
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

        public string Jop { get; set; }

        public bool Locked { get; set; }

        public UserTypeDTO UserType { get; set; }
    }
}