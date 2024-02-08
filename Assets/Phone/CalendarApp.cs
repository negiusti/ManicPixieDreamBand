using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CalendarApp : MonoBehaviour
{
    private SpriteResolver spriteResolver;
    private TextMeshPro tmp;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = this.GetComponentInChildren<SpriteResolver>();
        tmp = this.GetComponentInChildren<TextMeshPro>();
        UpdateImage();
    }

    void UpdateImage()
    {
        if (Calendar.IsNight())
        {
            spriteResolver.SetCategoryAndLabel("MoonSun", "moon");
        }
        else
        {
            spriteResolver.SetCategoryAndLabel("MoonSun", "sun");
        }
        tmp.text = Calendar.Date().ToString();
    }

    public void Sleep()
    {
        Calendar.Sleep();
        UpdateImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDayNight()
    {
        Calendar.ToggleDayNight();
        UpdateImage();
    }

    public void SetIsNight(bool value)
    {
        Calendar.SetIsNight(value);
        UpdateImage();
    }
}
