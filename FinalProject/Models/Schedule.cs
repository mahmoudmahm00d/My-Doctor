using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalProject.Models
{
    public class Schedule
    {
        [Key]
        [ForeignKey("Clinic")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClinicId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Days Day { get; set; }

        public Clinic Clinic { get; set; }
    }

    public enum Days
    {
        Saturday = 0, Sunday, Moday, Tuesday, Wednesday, Thursday, Friday
    }
}