using FinalProject.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class ClinicViewModel
    {
        [Required]
        [MaxLength(200)]
        public string ClinicName { get; set; }
        
        [Required]
        public byte ClinicTypeId { get; set; }

        public Doctor Doctor { get; set; }

        public string ClinicPhone { get; set; }

        public string ClinicEmail { get; set; }

        public LocationDTO Location { get; set; }
        public IEnumerable<ScheduleDTO> Schedule { get; set; }
    }
}