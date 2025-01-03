using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapsApp : PhoneApp
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
        ShoppingDistrict,
        Job1Neighborhood,
        TattooShop,
        BobaShop,
        Park,
        SkateShop
    };
    private GameManager gm;
    private SceneChanger sc;
    private Phone phone;
    private Pin[] pins;
    private TravelScreen travelScreen;
    private Location currentlySelectedLocation;
    private string currentSceneName;

    public static Dictionary<Location, Location> locationToNeighborhood = new Dictionary<Location, Location>
    {
        { Location.Bedroom, Location.DowntownNeighborhood },
        { Location.CoffeeShop, Location.DowntownNeighborhood },
        { Location.Basement, Location.DowntownNeighborhood },
        { Location.ThriftStore, Location.ShoppingDistrict },
        { Location.Bar, Location.ShoppingDistrict },
        { Location.MusicShop, Location.ShoppingDistrict },
        { Location.DowntownNeighborhood, Location.DowntownNeighborhood },
        { Location.ShoppingDistrict, Location.ShoppingDistrict },
        { Location.Job1Neighborhood, Location.Job1Neighborhood },
        { Location.TattooShop, Location.Job1Neighborhood },
        { Location.BobaShop, Location.Job1Neighborhood },
        { Location.SkateShop, Location.Job1Neighborhood },
        { Location.Park, Location.Park },};

    private List<string> GetLocationsInNeighborhood(Location neighborhoodLoc)
    {
        return locationToNeighborhood.Where(x => x.Value == neighborhoodLoc && x.Value != x.Key).Select(x => locationToNiceName[x.Key]).ToList();
    }

    private Dictionary<Location, string> locationToSceneName = new Dictionary<Location, string> {
        { Location.Bedroom, "Bedroom" },
        { Location.CoffeeShop, "CoffeeShop" },
        { Location.Basement, "BandPracticeRoom" },
        { Location.ThriftStore, "ThriftStore" },
        { Location.Bar, "SmallBar" },
        { Location.MusicShop, "MusicShop" },
        { Location.DowntownNeighborhood, "DowntownNeighborhood" },
        { Location.ShoppingDistrict, "ShoppingDistrict" },
        { Location.Job1Neighborhood, "Job1Neighborhood" },
        { Location.TattooShop, "TattooShop" },
        { Location.BobaShop, "BobaShop" },
        { Location.SkateShop, "SkateShop" },
        { Location.Park, "Park" },};

    public static Dictionary<string, Location> sceneNameToLocation = new Dictionary<string, Location> {
        { "Bedroom", Location.Bedroom },
        { "CoffeeShop", Location.CoffeeShop },
        { "BandPracticeRoom", Location.Basement },
        { "ThriftStore", Location.ThriftStore },
        { "SmallBar", Location.Bar },
        { "MusicShop", Location.MusicShop },
        { "DowntownNeighborhood", Location.DowntownNeighborhood },
        { "ShoppingDistrict", Location.ShoppingDistrict },
        { "Job1Neighborhood", Location.Job1Neighborhood },
        { "TattooShop", Location.TattooShop },
        { "BobaShop", Location.BobaShop },
        { "SkateShop", Location.SkateShop },
        { "Park", Location.Park },};

    private Dictionary<Location, string> locationToNiceName = new Dictionary<Location, string> {
        { Location.Bedroom, "Home" },
        { Location.CoffeeShop, "Coffee Zone" },
        { Location.Basement, "Basement" },
        { Location.ThriftStore, "Thrift Store" },
        { Location.Bar, "Dumpster Dive Bar" },
        { Location.MusicShop, "Music Shop" },
        { Location.DowntownNeighborhood, "Downtown" },
        { Location.ShoppingDistrict, "Shopping District" },
        { Location.Job1Neighborhood, "Capitol Valley" },
        { Location.TattooShop, "Tattoo Shop" },
        { Location.Park, "Al Canderson Park" },
        { Location.SkateShop, "Big Top Skate Shop" },
        { Location.BobaShop, "Cutie Boba Shop" }};

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
            Location currentNeighborhood = locationToNeighborhood[sceneNameToLocation[currentSceneName]];
            foreach (Pin p in pins)
            {
                if (p.location == currentNeighborhood)
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
        if (Phone.Instance == null)
            return;
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

    public void OpenPin(bool alreadyHere = false)
    {
        HidePins();
        phone.OpenPin();
        travelScreen.gameObject.SetActive(true);
        travelScreen.SetLocationName(locationToNiceName[currentlySelectedLocation], alreadyHere);
        travelScreen.SetLocationsList(GetLocationsInNeighborhood(currentlySelectedLocation));
    }

    private void ShowPins()
    {
        foreach (Pin p in pins)
        {
            p.gameObject.SetActive(true);
        }
        SetCurrentLocation();
    }

    private void HidePins()
    {
        foreach(Pin p in pins)
        {
            p.gameObject.SetActive(false);
        }
    }

    public override void Save()
    {
        // Nothing to do here
    }

    public override void Load()
    {
        // Nothing to do here
    }
}
