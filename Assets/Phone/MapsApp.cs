using System.Collections.Generic;
using UnityEngine;

public class MapsApp : MonoBehaviour
{
    public enum Location
    {
        Bedroom,
        Basement,
        CoffeeShop,
        ThriftStore
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
        { Location.ThriftStore, "ThriftStore" }};

    private Dictionary<string, Location> sceneNameToLocation = new Dictionary<string, Location> {
        { "Bedroom", Location.Bedroom },
        { "CoffeeShop", Location.CoffeeShop },
        { "BandPracticeRoom", Location.Basement },
        { "ThriftStore", Location.ThriftStore }};

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
        sc.ChangeScene(locationToSceneName[currentlySelectedLocation]);
        phone.GoHome();
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
        travelScreen.SetLocationName(currentlySelectedLocation.ToString());
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
