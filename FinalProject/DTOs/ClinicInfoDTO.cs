using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ClinicInfoDTO
    {
        public int ClinicId { get; set; }

        public ClinicTypeDTO ClinicType { get; set; }

        public Doctor Doctor { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClinicName { get; set; }

        public string ClinicPhone { get; set; }

        public string ClinicEmail { get; set; }
    }
}