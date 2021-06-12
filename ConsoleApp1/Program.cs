using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Device.Location;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GeoCoordinate geoFrom = new GeoCoordinate(33.499875917081,36.30239671838356);
            GeoCoordinate geoTo = new GeoCoordinate(33.50020496217854,36.29595170442295);

            Console.WriteLine(geoFrom);
            Console.WriteLine(geoTo);
            double destance = Math.Round(geoTo.GetDistanceTo(geoFrom));
            Console.WriteLine(destance+"m");
            //Time from = new Time("10:06");
            //Time to = new Time("17:10");
            //byte visitDuration = 15;
            //string[] times = GetTime(from.ToString(), to.ToString(), visitDuration);
            //foreach (var item in times)
            //{
            //    Console.WriteLine(item);
            //}
        }

        private static string[] GetTime(string fromTime, string toTime, byte visitDuration)
        {
            List<string> times = new List<string>();
            Time from = new Time(fromTime);
            Time to = new Time(toTime);
            int duration = Time.Differance(from, to);
            int count = duration / visitDuration;
            for (int i = 0; i <= count; i++)
            {
                times.Add(from.ToString());
                from = from + visitDuration;
            }
            return times.ToArray<string>();
        }
    }
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

        public static int Differance(Time from, Time to)
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

        //public static bool CheckTimeBetween(ScheduleDTO schedules, Schedule s)
        //{
        //    bool c1 = s.FromTime.ToDateTime() <= schedules.FromTime;
        //    bool c2 = schedules.FromTime <= s.ToTime.ToDateTime();

        //    bool c3 = s.FromTime.ToDateTime() <= schedules.ToTime;
        //    bool c4 = schedules.ToTime <= s.ToTime.ToDateTime();

        //    bool c5 = schedules.FromTime <= s.FromTime.ToDateTime();
        //    bool c6 = s.ToTime.ToDateTime() <= schedules.ToTime;

        //    //       From <= time <= To     TimeFrom <= From && To <= TimeTo
        //    return (c1 && c2) || (c3 && c4) || (c5 && c6);
        //}
    }

    public static class TimeExtension
    {
        public static DateTime ToDateTime(this string time)
        {
            Time t = new Time(time);
            DateTime date = DateTime.Now;
            return new DateTime(date.Year, date.Month, date.Day, t.Hour, t.Minute, 0);
        }

        //public static bool CheckTimeBetwwn(ScheduleDTO schedules, Schedule s)
        //{
        //    return s.FromTime.ToDateTime() <= schedules.FromTime
        //        && schedules.FromTime <= s.ToTime.ToDateTime();
        //}
    }
}
