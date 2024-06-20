using System;
using UnityEngine;

public class TattooJob : IJob
{
    public TattooJob()
    {
    }

    public bool IsNight()
    {
        return false;
    }

    public string Location()
    {
        return "Tattoo Shop";
    }

    public GameObject Minigame()
    {
        return MiniGameManager.GetMiniGame("Tattoo").gameObject;
    }

    public string Name()
    {
        return "Tattoo";
    }

    public JobSystem.PunkJob punkJob()
    {
        return JobSystem.PunkJob.Tattoo;
    }
}
