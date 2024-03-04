using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    //public enum StagePosition
    //{
    //    Left,
    //    Right,
    //    Center
    //};

    public GameObject leftAmp;
    public GameObject rightAmp;
    public PlayInstrument leftInst;
    public PlayInstrument rightInst;
    public PlayInstrument centerInst;
    public CrowdSpawner crowdSpawner;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartPerformance(Band band)
    {
        if (crowdSpawner != null)
            crowdSpawner.SpawnCrowd(band);
    }

    public void StopPerformance()
    {
        if (crowdSpawner != null)
            crowdSpawner.DespawnCrowd();
        leftInst.Stop();
        rightInst.Stop();
        centerInst.Stop();
    }

    public PlayInstrument GetInstrument(string position)
    {
        switch (position)
        {
            case "Left":
                return leftInst;
            case "Right":
                return rightInst;
            case "Center":
                return centerInst;
            default:
                return centerInst;
        }
    }
}