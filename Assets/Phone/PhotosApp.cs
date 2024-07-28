using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotosApp : PhoneApp
{
    private List<string> unlockedPhotos;
    public Photo photoTemplate;
    public Transform container;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        for (int i = 1; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
        // Start with two default unlocked photo
        unlockedPhotos = ES3.Load("Photos", new List<string>() { "Boxes", "PizzaRat" });
        foreach(string photoName in unlockedPhotos)
        {
            Photo photo = Instantiate(photoTemplate, container);
            photo.gameObject.SetActive(true);
            photo.AssignPhoto(photoName);
        }
    }

    private void OnDisable()
    {
        if (unlockedPhotos == null)
            return;
        ES3.Save("Photos", unlockedPhotos);
    }

    public void UnlockPhoto(string photoName)
    {
        Phone.Instance.SendNotificationTo("Photos");
        unlockedPhotos.Add(photoName);
    }
}
