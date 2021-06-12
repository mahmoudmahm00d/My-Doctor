using FinalProject.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }

        [DataType(DataType.Time)]
        public DateTime FromTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime ToTime { get; set; }

        [Range(0,6)]
        public DayOfWeek Day { get; set; }

        public string From
        {
            get => FromTime.ToTime();
        }
        public string To
        {
            get => ToTime.ToTime();
        }
    }
}