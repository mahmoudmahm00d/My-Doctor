using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Certifcate
    {
        public virtual User User { get; set; }
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        [StringLength(255)]
        public string CertifcateDescription { get; set; }
    }
}