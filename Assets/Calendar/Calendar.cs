using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Calendar", menuName = "Custom/Calendar")]
public class Calendar : ScriptableObject
{
    private static bool isNight;

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
        isNight = !isNight;
    }

    public static void SetIsNight(bool value)
    {
        isNight = value;
    }
}
