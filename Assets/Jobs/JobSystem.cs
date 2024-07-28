using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "JobSystem", menuName = "Custom/JobSystem")]
public class JobSystem : ScriptableObject
{
    public enum PunkJob
    {
        Unemployed,
        Tattoo,
        Boba
    };

    private static string saveKey = "CurrentPunkJob";
    private static string failStreakKey = "CurrentJobFailStreak";

    private static PunkJob currentJob = PunkJob.Unemployed;

    private static int failStreak; // two failed job performances prompts a fired text from your boss

    public static PunkJob CurrentJob()
    {
        return currentJob;
    }

    public static string CurrentJobString()
    {
        return currentJob.ToString();
    }

    public static IJob CurrentJobInfo()
    {
        if (currentJob == PunkJob.Tattoo)
            return new TattooJob();
        if (currentJob == PunkJob.Boba)
            return new BobaJob();
        return null;
    }

    public static void SetCurrentJob(PunkJob newJob)
    {
        if (newJob != currentJob)
        {
            failStreak = 0;
            Calendar.ScheduleNext7Days();
        }
        currentJob = newJob;
    }

    public static void GoodJob()
    {
        failStreak = Math.Max(failStreak - 1, 0);
    }

    public static void BadJob()
    {
        failStreak = Math.Min(failStreak + 1, 5);
        if (failStreak > 2)
        {
            // send fired streak
            Phone.Instance.ReceiveMsg("TXT/" + currentJob.ToString() + " Boss/Fire");
        }
    }

    public static void Save()
    {
        ES3.Save(saveKey, currentJob);
        ES3.Save(failStreakKey, failStreak);
    }

    public static void Load() {
        SetCurrentJob(ES3.Load(saveKey, PunkJob.Unemployed));
        failStreak = ES3.Load(failStreakKey, 0);
    }

    public static void SetCurrentJob(string newJob)
    {
        SetCurrentJob(stringToPunkJob(newJob));
    }

    private static PunkJob stringToPunkJob(string input)
    {
        PunkJob job = PunkJob.Unemployed;
        try
        {
            job = (PunkJob)Enum.Parse(typeof(PunkJob), input);
        }
        catch (ArgumentException)
        {
            Debug.LogError($"'{input}' is not a valid value for the PunkJob enum.");
        }
        return job;
    }


}
