using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ClinicOnlyDTO
    {
        public int ClinicId { get; set; }

        public ClinicTypeDTO ClinicType { get; set; }

        [Required]
        public byte ClinicTypeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClinicName { get; set; }

        public string ClinicPhone { get; set; }

        public string ClinicEmail { get; set; }
    }
}