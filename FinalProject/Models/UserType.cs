using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class UserType
    {
        public byte UserTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserTypeName { get; set; }
        public bool IsActiveUserType { get; set; }

        IEnumerable<User> Users { get; set; }
    }
}