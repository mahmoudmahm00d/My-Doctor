using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ClinicOnlyDTO
    {
        public int ClinicId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClinicName { get; set; }

        [Required]
        public byte ClinicTypeId { get; set; }

        public string ClinicPhone { get; set; }

        public string ClinicEmail { get; set; }
    }
}