using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Calendar", menuName = "Custom/Calendar")]
public class Calendar : ScriptableObject
{
    private static bool isNight; // is it day or night rn
    private static int day; // the current day
    private static Dictionary<int, List<ICalendarEvent>> events; // day number to list of events
    private static int currentEventIdx; // the first event of the day that has not already been completed

    private class CalendarData
    {
        public bool n;
        public int d;
        public int i;
        public Dictionary<int, List<ICalendarEvent>> e;
        public CalendarData(int d, bool n, int i, Dictionary<int, List<ICalendarEvent>> e)
        {
            this.n = n;
            this.d = d;
            this.i = i;
            this.e = e;
        }
    }

    public static void RemoveAllJobEvents()
    {
        for (int i = day; i < day + 7; i++)
        {
            Debug.Log("Unscheduling job for day " + i);
            if (events.ContainsKey(i))
            {
                events[i].RemoveAll(e => e is JobEvent);
            }
        }
    }

    public static void CompleteCurrentEvent()
    {
        if (currentEventIdx + 1 >= events.Count || events[day].Skip(currentEventIdx + 1).All(e => e.IsNight()))
        {
            SetIsNight(true);
        }
        currentEventIdx++;
        Weather.RandomizeWeather();
    }

    public static void SchedulePlotEvent(string eventName, string conversation, string location, bool isNight, double daysFromNow)
    {
        GetTodaysEvents();
        if (!events.ContainsKey(day + (int)daysFromNow))
            events.Add(day + (int)daysFromNow, new List<ICalendarEvent>());
        if (!events[day + (int)daysFromNow].Any(e => e.Name().Equals(eventName)))  // Don't schedule duplicate events!
        {
            events[day + (int)daysFromNow].Insert(0, new QuestEvent(eventName, conversation, isNight, location));
            // TODO: trigger calendar app notif
            if (daysFromNow == 0) // New event was added today!
                Phone.Instance.SendNotificationTo("Calendar");
        }
    }

    public static bool DoneForTheDay()
    {
        GetTodaysEvents();
        return currentEventIdx >= events[day].Count && events[day].Count > 0;
    }

    public static ICalendarEvent GetCurrentEvent()
    {
        if (events == null)
        {
            Load();
        }
        if (!events.ContainsKey(day))
        {
            ScheduleNext7Days();
        }
        return events[day].Count > currentEventIdx? events[day][currentEventIdx] : null;
    }

    public static int GetCurrentEventIdx()
    {
        return currentEventIdx;
    }

    public static List<ICalendarEvent> GetTodaysEvents()
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
            Debug.Log("Scheduling day " + i);
            if (!events.ContainsKey(i))
            {
                events.Add(i, new List<ICalendarEvent>());
            }

            if (isBandPracticeDay(i) && !isBandPracticeScheduled(i))
            {
                events[i].Add(new BandPracticeEvent(null, false));
            }
            if (isWorkScheduled(i) && JobSystem.CurrentJob() == JobSystem.PunkJob.Unemployed) {
                events[i].RemoveAll(e => e is JobEvent);
            }
            if (!isWorkScheduled(i) && JobSystem.CurrentJob() != JobSystem.PunkJob.Unemployed)
            {
                if (i != 3) // GIG DAY
                    events[i].Add(new JobEvent("Work", null, false, JobSystem.CurrentJobInfo().Location()));
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
        return events.ContainsKey(i) && events[i].Any(e => e is BandPracticeEvent);
    }

    private static bool isWorkScheduled(int i)
    {
        return events.ContainsKey(i) && events[i].Any(e => e is JobEvent);
    }

    private static bool isBandPracticeDay(int i)
    {
        if (i == 0 || i == 3)
            return false;
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
        DailyRandoms.Refresh();
        day++;
        SetIsNight(false);
        currentEventIdx = 0;
        ScheduleNext7Days();
        MainCharacterState.SetOutfitChangedFlag(false);
        InventoryManager.SpoilPerishables();
        Phone.Instance.SendNotificationTo("Calendar");
        if (day == 2)
            MainCharacterState.UnlockPhoto("PizzaRat");
    }

    public static int Date()
    {
        return day;
    }

    public static void SetIsNight(bool value)
    {
        isNight = value;
        FindFirstObjectByType<Sky>()?.UpdateSky();
    }


    public static void Save()
    {
        ES3.Save("Calendar", new CalendarData(day, isNight, currentEventIdx, events));
    }

    public static void Load()
    {
        if (!ES3.KeyExists("Calendar"))
        {
            events = new Dictionary<int, List<ICalendarEvent>>();
            SetIsNight(false);
            currentEventIdx = 0;
            day = 0;
            return;
        }
        CalendarData c = ES3.Load<CalendarData>("Calendar");
        day = c.d;
        SetIsNight(c.n);
        currentEventIdx = c.i;
        events = c.e ?? new Dictionary<int, List<ICalendarEvent>>();
    }

    public static void OnConversationComplete(string convoName)
    {
        // TODO: complete your event in the Dialogue tree
        //GetCurrentEvent()?.OnConversationComplete(convoName);
    }
}
