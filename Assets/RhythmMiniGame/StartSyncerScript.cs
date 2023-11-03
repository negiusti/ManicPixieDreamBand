using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSyncerScript : MonoBehaviour
{
    private AudioSource hamster;
    private float runwayDelay = 0f;
    private float startTime;
    private bool hasStarted = false;
 
    // Start is called before the first frame update
    void Start()
    {
        hamster = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                startTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator"))
        {
            runwayDelay = Time.time - startTime - .5f;
            hamster.Play();            
        }
    }

    public float GetRunwayDelay()
    {
        return runwayDelay;
    }
}
