using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        public UserDTO User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public string Time { get; set; }

        public bool Confirmed { get; set; }

        [Required]
        [StringLength(255)]
        public string Symptoms { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }
    }
}