using System;
using System.Linq;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "Custom/Tutorial")]
public class Tutorial : ScriptableObject
{
    public static bool hasWalked;
    public static bool hasSkated;
    public static bool changedSkin;
    public static bool hasClosedPhone;

    public static void Load()
    {
        hasWalked = ES3.Load("Tutorial/" + hasWalked, false);
        hasSkated = ES3.Load("Tutorial/" + hasSkated, false);
        changedSkin = ES3.Load("Tutorial/" + changedSkin, false);
        hasClosedPhone = ES3.Load("Tutorial/" + hasClosedPhone, false);
    }

    public static void Save()
    {
        ES3.Save("Tutorial/" + hasWalked, hasWalked);
        ES3.Save("Tutorial/" + hasSkated, hasSkated);
        ES3.Save("Tutorial/" + changedSkin, changedSkin);
        ES3.Save("Tutorial/" + hasClosedPhone, hasClosedPhone);
    }
}