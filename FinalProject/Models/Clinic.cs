using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Clinic
    {
        public int ClinicId { get; set; }

        [Required]
        [StringLength(100)]
        public string ClinicName { get; set; }

        public ClinicType ClinicType { get; set; }
        public byte ClinicTypeId { get; set; }

        public User ForUser { get; set; }
        public int UserId { get; set; }

        [StringLength(15)]
        public string ClinicPhone { get; set; }

        [StringLength(255)]
        public string ClinicEmail { get; set; }

        public string Certificate { get; set; }

        public byte VisitDuration { get; set; }

        public bool IsActiveClinic { get; set; }

        public Location Location { get; set; }
        public virtual IList<Schedule> Schedules { get; set; }
        public virtual IEnumerable<Appointment> Appointments { get; set; }
        public virtual IEnumerable<Vacation> Vacations { get; set; }
    }
}