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

    public static void RandomizeWeather()
    {
        Debug.Log("Weather is: " + currentWeather.ToString());
        // Get the number of values in the enum
        int enumLength = System.Enum.GetValues(typeof(WeatherState)).Length;

        // Generate a random index within the range of the enum length
        int randomIndex = Random.Range(0,enumLength);

        // Convert the random index to the corresponding enum value
        currentWeather = (WeatherState)randomIndex;
        Sky sky = FindFirstObjectByType<Sky>();
        if (sky != null)
            sky.UpdateSky();
        Debug.Log("Now weather is: " + currentWeather.ToString());
    }
}
