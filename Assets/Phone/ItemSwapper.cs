using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSwapper : MonoBehaviour
{
    private HashSet<Furniture> furniture;
    // Use this for initialization
    void Start()
    {

    }

    private void FindEditableItems ()
    {
        furniture = new HashSet<Furniture>(FindObjectsOfType<Furniture>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
