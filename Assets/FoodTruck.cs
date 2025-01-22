using UnityEngine;
using UnityEngine.U2D.Animation;

public class FoodTruck : MonoBehaviour
{
    public Character Parisa;
    public Character Emerita;
    public SpriteResolver cover;
    private SpriteResolver truck;

    // Start is called before the first frame update
    void Start()
    {
        truck = GetComponent<SpriteResolver>();
        // Select a random food truck
        switch (Random.Range(0,2))
        {
            case 0:
                SelectTruck("Filipino");
                break;
            default:
                SelectTruck("Persian");
                break;
        }
        if (Calendar.GetCurrentEvent() != null && Calendar.GetCurrentEvent().Name().ToLower().Contains("kickflip")) {
            SelectTruck("Persian");
        } else if (Calendar.Date() == 3 || Calendar.Date() == 7)
        {
            SelectTruck("Filipino");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectTruck(string selection)
    {
        if (truck == null)
            Start();
        switch (selection.ToLower())
        {
            case "filipino":
                Emerita.gameObject.SetActive(true);
                Parisa.gameObject.SetActive(false);
                truck.SetCategoryAndLabel("Truck", "Filipino");
                cover.SetCategoryAndLabel("Cover", "Filipino");
                break;
            default:
                Parisa.gameObject.SetActive(true);
                Emerita.gameObject.SetActive(false);
                truck.SetCategoryAndLabel("Truck", "Persian");
                cover.SetCategoryAndLabel("Cover", "Persian");
                break;
        }
    }
}
