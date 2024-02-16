using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BandsManager", menuName = "Custom/BandsManager")]
public class BandsManager : ScriptableObject
{
    private static Dictionary<string, int> bandToTixSales = new Dictionary<string, int> { { "poop", 2 } };

    public static int GetBandTixSales()
    {
        return Random.Range(1, 12);
    }

    public static int GetBandTixSales(string bandname)
    {
        return bandToTixSales[bandname];
    }

    public static void Save()
    {

    }

    public static void Load()
    {

    }
}
