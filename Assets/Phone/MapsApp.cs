using System.Collections.Generic;
using UnityEngine;

public class MapsApp : MonoBehaviour
{
    public enum Location
    {
        Bedroom,
        Basement,
        CoffeeShop,
        ThriftStore,
        Bar,
        MusicShop,
        DowntownNeighborhood,
        ShoppingDistrict
    };
    private GameManager gm;
    private SceneChanger sc;
    private Phone phone;
    private Pin[] pins;
    private TravelScreen travelScreen;
    private Location currentlySelectedLocation;
    private string currentSceneName;

    private Dictionary<Location, string> locationToSceneName = new Dictionary<Location, string> {
        { Location.Bedroom, "Bedroom" },
        { Location.CoffeeShop, "CoffeeShop" },
        { Location.Basement, "BandPracticeRoom" },
        { Location.ThriftStore, "ThriftStore" },
        { Location.Bar, "SmallBar" },
        { Location.MusicShop, "MusicShop" },
        { Location.DowntownNeighborhood, "DowntownNeighborhood" },
        { Location.ShoppingDistrict, "ShoppingDistrict" },};

    private Dictionary<string, Location> sceneNameToLocation = new Dictionary<string, Location> {
        { "Bedroom", Location.Bedroom },
        { "CoffeeShop", Location.CoffeeShop },
        { "BandPracticeRoom", Location.Basement },
        { "ThriftStore", Location.ThriftStore },
        { "SmallBar", Location.Bar },
        { "MusicShop", Location.MusicShop },
        { "DowntownNeighborhood", Location.DowntownNeighborhood },
        { "ShoppingDistrict", Location.ShoppingDistrict }};

    private Dictionary<Location, string> locationToNiceName = new Dictionary<Location, string> {
        { Location.Bedroom, "Home" },
        { Location.CoffeeShop, "Coffee Shop" },
        { Location.Basement, "Basement" },
        { Location.ThriftStore, "Thrift Store" },
        { Location.Bar, "Bar" },
        { Location.MusicShop, "Music Shop" },
        { Location.DowntownNeighborhood, "Downtown" },
        { Location.ShoppingDistrict, "Shopping District" },};

    // Start is called before the first frame update
    void Start()
    {
        pins = this.GetComponentsInChildren<Pin>();
        gm = GameManager.Instance;
        sc = gm.gameObject.GetComponent<SceneChanger>();
        phone = this.GetComponentInParent<Phone>();
        travelScreen = this.GetComponentInChildren<TravelScreen>();
        SetCurrentLocation();
    }

    private void SetCurrentLocation()
    {
        currentSceneName = sc.GetCurrentScene();
        if (sceneNameToLocation.ContainsKey(currentSceneName))
        {
            Location l = sceneNameToLocation[currentSceneName];
            foreach (Pin p in pins)
            {
                if (p.location == l)
                {
                    p.SetHere();
                }
                else
                {
                    p.SetNotHere();
                }
            }
        }
        else
        {
            Debug.LogError(currentSceneName + " is not inside sceneNameToLocationMap");
        }
    }

    private void OnEnable()
    {
        if (sc == null)
            return;
        SetCurrentLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToSelectedLocation()
    {
        Debug.Log("currentlySelectedLocation: " + currentlySelectedLocation);
        sc.ChangeScene(locationToSceneName[currentlySelectedLocation], SceneChanger.LoadingScreenType.Bus);
        phone.Lock();
    }

    public void Open()
    {
        ShowPins();
        travelScreen.gameObject.SetActive(false);
    }

    public void SetLocation(Location location)
    {
        currentlySelectedLocation = location;
    }

    public void OpenPin()
    {
        HidePins();
        phone.OpenPin();
        travelScreen.gameObject.SetActive(true);
        travelScreen.SetLocationName(locationToNiceName[currentlySelectedLocation]);
    }

    private void ShowPins()
    {
        foreach (Pin p in pins)
        {
            p.gameObject.SetActive(true);
        }
    }

    private void HidePins()
    {
        foreach(Pin p in pins)
        {
            p.gameObject.SetActive(false);
        }
    }
}
