
using System.Collections.Generic;
using UnityEngine;

/*
 * Tuning Strings class in charge of moving from string to string. 
 */
public class TuningStrings : MonoBehaviour
{
   
    public GuitarString string1;
    public GuitarString string2;
    public GuitarString string3;
    public GuitarString string4;
    public int indexOfString = 0;
    public TunerNote tuner;
    public static float targetValue = 0;
    List<GuitarString> guitarStrings = new List<GuitarString>();
    
  
    void Start()
    {
        guitarStrings.Add(string1);
        guitarStrings.Add(string2);
        guitarStrings.Add(string3);
        guitarStrings.Add(string4);

        if (guitarStrings.Count > 0)
        {
            guitarStrings[indexOfString].Select();
        }
        
        
    }

    public void NextString()
    {
        if (indexOfString < guitarStrings.Count - 1)
        {
            guitarStrings[indexOfString].Unselect();
            indexOfString++;
            guitarStrings[indexOfString].Select();
          
        } else
        {
            guitarStrings[indexOfString].Unselect();
            tuner.ResetTextToBlank();
            tuner.SetEndScreen();
        }

        
            
    }




}
