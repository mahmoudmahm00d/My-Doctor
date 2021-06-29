using FinalProject.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FromTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
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