using UnityEngine;
using UnityEngine.U2D.Animation;

public class Sky : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    private ParticleSystem rain;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = this.GetComponent<SpriteResolver>();
        rain = this.GetComponentInChildren<ParticleSystem>(includeInactive: true);
    }

    public void UpdateSky()
    {
        Weather.WeatherState currentWeather = Weather.Current();
        switch (currentWeather)
        {
            case Weather.WeatherState.Rainy:
                rain.gameObject.SetActive(true);
                break;
            default:
                rain.gameObject.SetActive(false);
                break;
        }
        string label = Calendar.IsDay() ? "Day" : "Night";
        if (spriteResolver == null)
            Start();
        spriteResolver.SetCategoryAndLabel("Sky", label);
    }

    private void OnEnable()
    {
        Start();
        UpdateSky();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
