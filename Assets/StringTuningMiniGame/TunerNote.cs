
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
 * Tuner class that holds notes display, frequency, endscreen, and background. 
 */
public class TunerNote : MonoBehaviour
{
    // E,A,D,G variables are note displays on tuner
    public TMP_Text text;
    public TMP_Text numbers;
    public TMP_Text endScreen;
    public GameObject E;
    public GameObject A;
    public GameObject D;
    public GameObject G;
    public List<GameObject> noteDisplays = new List<GameObject>();
    public GameObject TunerBackground;
     

    void Start()
    {

        noteDisplays.Add(E);
        noteDisplays.Add(A);
        noteDisplays.Add(D);
        noteDisplays.Add(G);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Methods for Displaying and Undisplaying the notes on the tuner
    public void DisplayNote(GameObject noteDisplay)
    {
        noteDisplay.SetActive(true); 
    }
    public void UnDisplayNote(GameObject noteDisplay)
    {
        noteDisplay.SetActive(false);
    }

    public void SetNote(string note)
    {
       text.SetText("Tuning:" + note );
    }

    public void SetValue(float val)
    {
        numbers.SetText("Frequency:" + val);
    }
    public void ResetTextToBlank()
    {
        text.SetText("   ");
        numbers.SetText("    ");
    }
    public void SetEndScreen()
    {
        endScreen.SetText("Your guitar is tuned! Time Elasped: " + Time.time);
    }



    
}

