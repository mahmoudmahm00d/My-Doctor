using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Certifcate
    {
        public User User { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CertifcateID { get; set; }

        public int UserId { get; set; }

        [Required]
        public string CertifcateDescription { get; set; }
    }
}