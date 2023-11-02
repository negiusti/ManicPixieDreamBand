using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSyncerScript : MonoBehaviour
{
    private float minY = 0f;
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
        //if (!alreadyPlaying && this.transform.position.y < minY)
        //{
            //hamster.Play();
            //alreadyPlaying = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator"))
        {
            hamster.Play();
        }
    }
}
