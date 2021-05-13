using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalProject.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }

        public Days Day { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Clinic Clinic { get; set; }
    }

    public enum Days
    {
        Saturday = 0, Sunday, Moday, Tuesday, Wednesday, Thursday, Friday
    }
}