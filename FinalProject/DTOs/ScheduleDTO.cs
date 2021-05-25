using FinalProject.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }

        [DataType(DataType.Time)]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime ToDate { get; set; }
        public Days Day { get; set; }
    }
}