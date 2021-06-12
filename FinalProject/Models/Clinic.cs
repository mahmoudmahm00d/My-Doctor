using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Clinic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public IList<Schedule> Schedules { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<Vacation> Vacations { get; set; }
    }
}