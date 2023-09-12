using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSyncerScript : MonoBehaviour
{
    private float minX = -7.47f;
    private AudioSource hamster;
    private bool alreadyPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        hamster = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!alreadyPlaying && this.transform.position.x < minX)
        {
            hamster.Play();
            alreadyPlaying = true;
        }
    }
}
