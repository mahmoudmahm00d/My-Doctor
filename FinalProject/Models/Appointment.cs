using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public Clinic Clinic { get; set; }
        public int ClinicId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

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

        public virtual IEnumerable<Prescription> Prescriptions { get; set; }
    }
}