using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class UserType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte UserTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserTypeName { get; set; }
        public bool IsActiveUserType { get; set; }

        IEnumerable<User> Users { get; set; }
    }
}