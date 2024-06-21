using System;
using UnityEngine;

public class BobaJob : IJob
{
    public BobaJob()
    {
    }

    public bool IsNight()
    {
        return false;
    }

    public string Location()
    {
        return "Boba Shop";
    }

    public GameObject Minigame()
    {
        return MiniGameManager.GetMiniGame("Boba").gameObject;
    }

    public string Name()
    {
        return "Boba";
    }

    public JobSystem.PunkJob punkJob()
    {
        return JobSystem.PunkJob.Boba;
    }
}
