using UnityEngine;
using UnityEngine.U2D.Animation;

public class Sky : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    private ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = this.GetComponent<SpriteResolver>();
        particles = this.GetComponentInChildren<ParticleSystem>();
    }

    public void UpdateSky()
    {
        Weather.WeatherState currentWeather = Weather.Current();
        switch (currentWeather)
        {
            case Weather.WeatherState.Rainy:
                particles.gameObject.SetActive(true);
                break;
            default:
                particles.gameObject.SetActive(false);
                break;
        }
        string label = Calendar.IsDay() ? "Day" : "Night";
        if (spriteResolver == null)
            Start();
        spriteResolver.SetCategoryAndLabel("Sky", label);
    }

    private void OnEnable()
    {
        UpdateSky();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
