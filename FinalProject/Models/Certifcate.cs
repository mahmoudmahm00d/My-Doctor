using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Certifcate
    {
        public virtual User User { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        [StringLength(255)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CertifcateDescription { get; set; }
    }
}