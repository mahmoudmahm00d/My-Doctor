using FinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ClinicDTO
    {
        [Required]
        [MaxLength(200)]
        public string ClinicName { get; set; }
        
        [Required]
        public byte ClinicTypeId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string ClinicPhone { get; set; }

        public string ClinicEmail { get; set; }

        public Location Location { get; set; }
        public Schedule Schedule { get; set; }
    }
}