using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;
using System.Text;
using System.Globalization;


namespace ZadanieSpotkajmySie
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test();

            string jsonCalendar1 = Console.ReadLine();
            string jsonCalendar2 = Console.ReadLine();
            TimeSpan meetingDuration;
            TimeSpan.TryParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture, out meetingDuration);

            Calendar c1 = JsonSerializer.Deserialize<Calendar>(jsonCalendar1);
            Calendar c2 = JsonSerializer.Deserialize<Calendar>(jsonCalendar2);

            List<TimeInterval> result = DeterminePossibleTimeIntervalsForMeetings(c1, c2, meetingDuration);
            string output = result.Select(o => $"[\"{o.start.ToString("HH:mm")}\",\"{o.end.ToString("HH:mm")}\"]").Aggregate((o1, o2) => o1 + ", " + o2);
            Console.WriteLine(output);

        }

        static List<TimeInterval> DeterminePossibleTimeIntervalsForMeetings(Calendar c1, Calendar c2, TimeSpan meetingDuration)
        {
            var earliestPossibleStart = c1.working_hours.start > c2.working_hours.start ? c1.working_hours.start : c2.working_hours.start;
            var latestPossibleEnd = c1.working_hours.end < c2.working_hours.end ? c1.working_hours.end : c2.working_hours.end;

            var allMeetingLists = new List<TimeInterval>() { new TimeInterval { start = earliestPossibleStart, end = earliestPossibleStart } };

            allMeetingLists.AddRange(c1.planned_meeting);
            allMeetingLists.AddRange(c2.planned_meeting);
            allMeetingLists.Add(new TimeInterval { start = latestPossibleEnd, end = latestPossibleEnd });

            var allMeetingsListOrdered = allMeetingLists.Where(x => x.start >= earliestPossibleStart && x.end <= latestPossibleEnd).OrderBy(x => x.start).ToList();

            DateTimeOffset max = DateTimeOffset.MinValue;
            var mergedList = new List<TimeInterval>();

            for (int i = 0; i < allMeetingsListOrdered.Count(); i++)
            {

                if (max > allMeetingsListOrdered[i].end)
                {
                    continue;
                }
                else
                {
                    mergedList.Add(allMeetingsListOrdered[i]);
                    max = allMeetingsListOrdered[i].end;
                }
            }

            var possibleMeetingTimeIntervals = new List<TimeInterval>();
            for (int i = 0; i < mergedList.Count() - 1; i++)
            {
                if (mergedList[i + 1].start - mergedList[i].end >= meetingDuration)
                {
                    possibleMeetingTimeIntervals.Add(new TimeInterval { start = mergedList[i].end, end = mergedList[i + 1].start });
                }

            }


            return possibleMeetingTimeIntervals;

        }

        static void Test()
        {
            string json1 = @"{ ""working_hours"" : { ""start"":""09:00"",""end"":""19:55""}, ""planned_meeting"": [{""start"":""09:00"",""end"":""10:30""}, {""start"":""12:00"",""end"":""13:00""},{""start"":""16:00"",""end"":""18:00""}]}";
            string json2 = @"{ ""working_hours"" : { ""start"":""10:00"",""end"":""18:30""}, ""planned_meeting"": [{""start"":""10:00"",""end"":""11:30""}, {""start"":""12:30"",""end"":""14:30""},{""start"":""14:30"",""end"":""15:00""},{""start"":""16:00"",""end"":""17:00""} ]}";
            Calendar c1 = JsonSerializer.Deserialize<Calendar>(json1);
            Calendar c2 = JsonSerializer.Deserialize<Calendar>(json2);

            List<TimeInterval> result = DeterminePossibleTimeIntervalsForMeetings(c1, c2, TimeSpan.Parse("00:30"));
            string output = result.Select(o => $"[\"{o.start.ToString("HH:mm")}\",\"{o.end.ToString("HH:mm")}\"]").Aggregate((o1, o2) => o1 + ", " + o2);

            Console.WriteLine(output);
        }
    }
}
