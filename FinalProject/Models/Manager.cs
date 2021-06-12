using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }

        [Required]
        [StringLength(255)]
        public string ManagerEmail { get; set; }

        [Required]
        [StringLength(255)]
        public string ManagerPassword { get; set; }

        [StringLength(10)]
        public string VerCode { get; set; }
    }
}