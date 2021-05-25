using FinalProject.DTOs;
using System.Collections.Generic;

namespace FinalProject.ViewModels
{
    public class AddDayViewModel
    {
        public AddDayViewModel()
        {
            List<Day> l = new List<Day>();
            l.Add(new Day { Value = 0, Name = "Saturday" });
            l.Add(new Day { Value = 1, Name = "Sunday" });
            l.Add(new Day { Value = 2, Name = "Moday" });
            l.Add(new Day { Value = 3, Name = "Tuesday" });
            l.Add(new Day { Value = 4, Name = "Wednesday" });
            l.Add(new Day { Value = 5, Name = "Thursday" });
            l.Add(new Day { Value = 6, Name = "Friday" });
            SelectDays = l;
        }
        public ScheduleDTO Day { set; get; }
        public IEnumerable<Day> SelectDays { set; get; }
    }

    public class Day
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}