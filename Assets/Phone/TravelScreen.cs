using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class TravelScreen : MonoBehaviour
{
    public TextMeshPro startNavText;
    public TextMeshPro locationText;
    private SpriteResolver iconResolver;
    public TextMeshPro locationsList;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        iconResolver = GetComponentInChildren<SpriteResolver>(true);
        button = GetComponentInChildren<Button>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLocationName(string location, bool alreadyHere = false)
    {
        if (alreadyHere)
        {
            startNavText.text = "You're already here!";
            locationText.text = location;
            button.gameObject.SetActive(false);
        } else
        {
            startNavText.text = "Start navigation to";
            locationText.text = location + "?";
            button.gameObject.SetActive(true);
        }
        
        iconResolver.SetCategoryAndLabel("Neighborhood", location);
    }

    public void SetLocationsList(List<string> ll)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(string l in ll)
        {
            stringBuilder.Append("* " + l + "\n");
        }
        
        locationsList.text = stringBuilder.ToString();
    }
}
