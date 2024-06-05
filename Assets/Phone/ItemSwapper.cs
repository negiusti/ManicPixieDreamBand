using UnityEngine;
using System.Collections.Generic;

public class ItemSwapper : MonoBehaviour
{
    protected HashSet<Furniture> furniture;
    // Use this for initialization
    void Start()
    {

    }

    protected void FindEditableItems ()
    {
        furniture = new HashSet<Furniture>(FindObjectsOfType<Furniture>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
