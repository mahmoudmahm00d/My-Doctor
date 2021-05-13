using FinalProject.Models;
using System;

namespace FinalProject.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Days Day { get; set; }
    }
}