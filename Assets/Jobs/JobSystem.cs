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

    private static PunkJob currentJob = PunkJob.Unemployed;

    public static PunkJob CurrentJob()
    {
        return PunkJob.Tattoo;
        //return currentJob;
    }

    public static void SetCurrentJob(PunkJob newJob)
    {
        currentJob = newJob;
    }
}
