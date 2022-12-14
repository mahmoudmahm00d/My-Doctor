using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalProject.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }

        public DayOfWeek Day { get; set; }

        [Required]
        [StringLength(8)]
        public string FromTime { get; set; }

        [Required]
        [StringLength(8)]
        public string ToTime { get; set; }

        public Clinic Clinic { get; set; }
    }

    //public enum Days
    //{
    //    Saturday = 0, Sunday, Moday, Tuesday, Wednesday, Thursday, Friday
    //}
}