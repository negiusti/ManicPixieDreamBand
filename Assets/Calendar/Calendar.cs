using UnityEngine;

[CreateAssetMenu(fileName = "Calendar", menuName = "Custom/Calendar")]
public class Calendar : ScriptableObject
{
    private static bool isNight;
    private static int day;

    private class CalendarData
    {
        public bool n;
        public int d;
        public CalendarData(int d, bool n)
        {
            this.n = n;
            this.d = d;
        }
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
        ES3.Save("Calendar", new CalendarData(day, isNight));
    }

    public static void Load()
    {
        if (!ES3.KeyExists("Calendar"))
            return;
        CalendarData c = ES3.Load<CalendarData>("Calendar");
        day = c.d;
        isNight = c.n;
    }
}
