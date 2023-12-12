using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsApp : MonoBehaviour
{
    private GameManager gm;
    private SceneChanger sc;
    private Phone phone;
    private Pin[] pins;
    private string currentlySelectedLocation;

    // Start is called before the first frame update
    void Start()
    {
        pins = this.GetComponentsInChildren<Pin>();
        gm = GameManager.Instance;
        sc = gm.gameObject.GetComponent<SceneChanger>();
        phone = this.GetComponentInParent<Phone>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        ShowPins();
    }

    public void SetLocation(string location)
    {
        currentlySelectedLocation = location;
    }

    public void OpenPin()
    {
        HidePins();
        phone.OpenPin();
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
