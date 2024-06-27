using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaCup : MonoBehaviour
{
    public GameObject straw;
    public Color[] strawColors;
    public GameObject liquid;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        straw.GetComponent<SpriteRenderer>().color = strawColors[Random.Range(0, strawColors.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
