using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }

        [Required]
        [StringLength(255)]
        public string ManagerEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string ManagerPassword { get; set; }

        [StringLength(10)]
        public string VerCode { get; set; }
    }
}