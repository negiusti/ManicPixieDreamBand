using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TravelScreen : MonoBehaviour
{
    public TextMeshPro locationText;
    //private MapsApp mapsApp;

    // Start is called before the first frame update
    void Start()
    {
        //mapsApp = this.GetComponentInParent<MapsApp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLocationName(string location)
    {
        locationText.text = location + " ?";
    }
}
