using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BandsManager", menuName = "Custom/BandsManager")]
public class BandsManager : ScriptableObject
{
    //private static Dictionary<string, int> bandToTixSales = new Dictionary<string, int> { { "poop", 2 } };

    public static Band GetBandByName(string bandName)
    {
        return BandJson.GetBandsData().First(b => b.Name == bandName);
    }


    public static int GetBandTixSales(int avg)
    {
        return avg;
        //return Random.Range(Mathf.Max(1, avg -2), Mathf.Max(avg + 2, 12));
    }

    //public static int GetBandTixSales(string bandname)
    //{
    //    return bandToTixSales[bandname];
    //}

    public static void Save()
    {

    }

    public static void Load()
    {

    }
}
