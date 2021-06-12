using System;
using System.Collections.Generic;

namespace FinalProject.DTOs
{
    public class AvailableAppointments
    {
        public List<AvailableDay> AvailableDays { get; set; }
    }

    public class AvailableDay
    {
        public DayOfWeek Day { get; set; }
        public List<string> Times { get; set; }
    }
}