using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class TravelScreen : MonoBehaviour
{
    public TextMeshPro locationText;
    private SpriteResolver iconResolver;
    public TextMeshPro locationsList;
    // Start is called before the first frame update
    void Start()
    {
        iconResolver = GetComponentInChildren<SpriteResolver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLocationName(string location)
    {
        locationText.text = location + " ?";
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
