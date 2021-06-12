using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        public Clinic Clinic { get; set; }
        public int ClinicId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }

        public bool Confirmed { get; set; }

        [StringLength(255)]
        public string Symptoms { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        public IEnumerable<Prescription> Prescriptions { get; set; }
    }
}