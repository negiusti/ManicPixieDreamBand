using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Calendar", menuName = "Custom/Calendar")]
public class Calendar : ScriptableObject
{
    private static bool isNight; // is it day or night rn
    private static int day; // the current day
    private static Dictionary<int, List<CalendarEvent>> events; // day number to list of events
    private static int currentEventIdx; // the first event of the day that has not already been completed

    private class CalendarData
    {
        public bool n;
        public int d;
        public int i;
        public Dictionary<int, List<CalendarEvent>> e;
        public CalendarData(int d, bool n, int i, Dictionary<int, List<CalendarEvent>> e)
        {
            this.n = n;
            this.d = d;
            this.i = i;
            this.e = e;
        }
    }
    public static void CompleteCurrentEvent()
    {
        if (currentEventIdx + 1 >= events.Count || events[day].Skip(currentEventIdx + 1).All(e => e.IsNight()))
        {
            SetIsNight(true);
        }
        currentEventIdx++;
    }

    public static int GetCurrentEventIdx()
    {
        return currentEventIdx;
    }

    public static List<CalendarEvent> GetTodaysEvents()
    {
        if (events == null)
        {
            Load();
        }
        if (!events.ContainsKey(day))
        {
            ScheduleNext7Days();
        }
        return events[day];
    }

    public static void ScheduleNext7Days()
    {
        for (int i = day; i < day + 7; i++)
        {
            if (!events.ContainsKey(i))
            {
                events.Add(i, new List<CalendarEvent>());
            }

            if (isBandPracticeDay(i) && !isBandPracticeScheduled(i))
            {
                events[i].Add(new BandPracticeEvent(null, null, null, false));
            }
            if (!isWorkScheduled(i))
            {
                if (i %2 == 0)
                    events[i].Add(new JobEvent("werk", null, null, null, false, "Coffee Shop"));
                else
                    events[i].Add(new JobEvent("werk", null, null, null, true, "Bar"));
            }
            

            //// Already booked
            //if (events[i].Count >= 2)
            //{
            //    continue;
            //}
            //else if (events[i].Count == 1)
            //{
            //    // schedule one more event
            //    if (events[i][0].IsNight())
            //    {
            //        // schedule a day event
            //    }
            //    else
            //    {
            //        // schedule a night event
            //    }
            //}
            //else
            //{
            //    // schedule two events: one evening, one day
            //}
        }
    }

    private static bool isBandPracticeScheduled(int i)
    {
        return events.ContainsKey(i) ? events[i].Any(e => e is BandPracticeEvent) : false;
    }

    private static bool isWorkScheduled(int i)
    {
        return events.ContainsKey(i) ? events[i].Any(e => e is JobEvent) : false;
    }

    private static bool isBandPracticeDay(int i)
    {
        return true;
    }

    public static void ScheduleGig()
    {

    }

    public static bool IsNight()
    {
        return isNight;
    }

    public static bool IsDay()
    {
        return !isNight;
    }

    public static void ToggleDayNight()
    {
        SetIsNight(!isNight);
    }

    public static void Sleep()
    {
        day++;
        SetIsNight(false);
        currentEventIdx = 0;
        ScheduleNext7Days();
    }

    public static int Date()
    {
        return day;
    }

    public static void SetIsNight(bool value)
    {
        isNight = value;
    }


    public static void Save()
    {
        ES3.Save("Calendar", new CalendarData(day, isNight, currentEventIdx, events));
    }

    public static void Load()
    {
        if (!ES3.KeyExists("Calendar"))
        {
            events = new Dictionary<int, List<CalendarEvent>>();
            SetIsNight(false);
            currentEventIdx = 0;
            day = 0;
            return;
        }
        CalendarData c = ES3.Load<CalendarData>("Calendar");
        day = c.d;
        isNight = c.n;
        currentEventIdx = c.i;
        events = c.e;
    }
}
