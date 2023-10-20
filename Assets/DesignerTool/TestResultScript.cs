using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResultScript : MonoBehaviour
{
    public Camera cam;
    public SpriteCombiner sc;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sr.sprite = sc.CombineSprites(cam);
        }
    }
}
