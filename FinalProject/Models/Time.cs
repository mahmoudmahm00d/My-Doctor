using FinalProject.DTOs;
using System;

namespace FinalProject.Models
{
    public class Time
    {
        int hour;
        int minute;

        public int Hour { get => hour; }
        public int Minute { get => minute; }
        public Time(string time)//00:00
        {
            string[] hhmm = time.Split(':');
            hour = int.Parse(hhmm[0]);
            minute = int.Parse(hhmm[1]);
        }

        public Time(DateTime dateTime)
        {
            hour = dateTime.Hour;
            minute = dateTime.Minute;
        }

        public static string ToString(DateTime dateTime)
        {
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            return $"{hour.ToString().PadLeft(2, '0')}:{minute.ToString().PadLeft(2, '0')}";
        }

        public int CompareTo(Time other)
        {
            if (hour != other.hour)
                return hour.CompareTo(other.hour);
            return minute.CompareTo(other.minute);
        }

        public override string ToString()
        {
            return $"{hour.ToString().PadLeft(2, '0')}:{minute.ToString().PadLeft(2, '0')}";
        }

        public static int Differance(string from, string to)
        {
            Time fromTime = new Time(from);
            Time toTime = new Time(to);
            int fromInMinutes = fromTime.hour * 60 + fromTime.minute;
            int toInMinutes = toTime.hour * 60 + toTime.minute;
            return toInMinutes - fromInMinutes;
        }

        public static int Differance(Time from,Time to)
        {
            int fromInMinutes = from.hour * 60 + from.minute;
            int toInMinutes = to.hour * 60 + to.minute;
            return toInMinutes - fromInMinutes; 
        }

        public static Time operator +(Time time, int minute)
        {
            time.minute += minute;
            if (time.minute > 59)
            {
                time.minute -= 60;
                time.hour += 1;
            }
            if (time.hour > 23)
            {
                time.hour -= 24;
            }
            return time;
        }

        public static bool operator <=(Time t1, Time t2)
        {
            if (t1.CompareTo(t2) == 1)
                return false;
            return true;
        }

        public static bool operator >=(Time t1, Time t2)
        {
            if (t2.CompareTo(t2) == 1)
                return false;
            return true;
        }

        public static bool CheckTimeBetween(ScheduleDTO schedules, Schedule s)
        {
            bool c1 = s.FromTime.ToDateTime() <= schedules.FromTime;
            bool c2 = schedules.FromTime <= s.ToTime.ToDateTime();

            bool c3 = s.FromTime.ToDateTime() <= schedules.ToTime;
            bool c4 = schedules.ToTime <= s.ToTime.ToDateTime();

            bool c5 = schedules.FromTime  <= s.FromTime.ToDateTime();
            bool c6 = s.ToTime.ToDateTime() <= schedules.ToTime;

            //       From <= time <= To     TimeFrom <= From && To <= TimeTo
            return (c1 && c2) || (c3 && c4) || (c5 && c6);
        }
    }

    public static class TimeExtension
    {
        public static DateTime ToDateTime(this string time)
        {
            Time t = new Time(time);
            DateTime date = DateTime.Now;
            return new DateTime(date.Year, date.Month, date.Day, t.Hour, t.Minute, 0);
        }

        public static string ToTime(this DateTime time)
        {
            Time t = new Time(time);
            return t.ToString();
        }

        public static bool CheckTimeBetwwn(ScheduleDTO schedules, Schedule s)
        {
            return s.FromTime.ToDateTime() <= schedules.FromTime
                && schedules.FromTime <= s.ToTime.ToDateTime();
        }
    }
}