using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotosApp : PhoneApp
{
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
        foreach(string photoName in MainCharacterState.UnlockedPhotos())
        {
            Photo photo = Instantiate(photoTemplate, container);
            photo.gameObject.SetActive(true);
            photo.AssignPhoto(photoName);
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
