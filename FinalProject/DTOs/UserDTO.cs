using FinalProject.Models;

namespace FinalProject.DTOs
{
    public class UserDTO
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

        private Genders gender;

        public string Gender
        {
            get { return gender == Genders.Male ? "Male" : "Female"; }
        }

        public string Jop { get; set; }

        public byte UserTypeId { get; set; }
    }
}