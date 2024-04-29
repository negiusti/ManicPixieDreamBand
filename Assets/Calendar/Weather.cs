using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Weather", menuName = "Custom/Weather")]
public class Weather : ScriptableObject
{
   public enum WeatherState
    {
        Cloudy,
        Rainy,
        Clear
    }

    private static WeatherState currentWeather;

    public static WeatherState Current()
    {
        return currentWeather;
    }

}
