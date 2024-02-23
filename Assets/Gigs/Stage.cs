using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public enum StagePosition
    {
        Left,
        Right,
        Center
    };

    public GameObject leftAmp;
    public GameObject rightAmp;
    public PlayInstrument leftInst;
    public PlayInstrument rightInst;
    public PlayInstrument centerInst;
    private Character[] performers;

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
        

    }

    public void StopPerformance()
    {


    }
}