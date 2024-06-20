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

    private static PunkJob currentJob = PunkJob.Unemployed;

    public static PunkJob CurrentJob()
    {
        return currentJob;
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
        currentJob = newJob;
    }

    public static void Save()
    {
        ES3.Save(saveKey, currentJob);
    }

    public static void Load() {
        SetCurrentJob(ES3.Load(saveKey, PunkJob.Unemployed));
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
