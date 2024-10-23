using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.Models
{
    public static class TimetableValidationUtils
    {
        public static bool IsValidDateTimeFormat(DateTimeOffset dateTime)
        {
            string format = "yyyy-MM-ddTHH:mm:ssZ";
            string dateTimeString = dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

            return DateTimeOffset.TryParseExact(dateTimeString, format, null, System.Globalization.DateTimeStyles.AssumeUniversal, out _);
        }

        public static bool IsValidDateTimeOffset(DateTimeOffset dateTime)
        {
            return dateTime.Minute % 30 == 0;
        }

        public static bool IsValidDateTime(DateTime dateTime)
        {
            return dateTime.Minute % 30 == 0 && dateTime.Second == 0;
        }

        public static bool CheckForOverlapping(Entitys.Timetable timetable,
            List<Entitys.Timetable> existingTimetables)
        {
            return !existingTimetables.Any(existing =>
                (timetable.From >= existing.From && timetable.From < existing.To) ||
                (timetable.To > existing.From && timetable.To <= existing.To) ||
                (timetable.From <= existing.From && timetable.To >= existing.To)
            );
        }
    }
}

