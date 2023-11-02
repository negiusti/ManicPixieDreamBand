using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMoverScript : MonoBehaviour
{
    private float beatTempo = 4f;
    public bool hasStarted = false;
    private float startTime;
    private float runwayDelay;


    // Start is called before the first frame update
    void Start()
    {
        if (hasStarted)
            startTime = Time.time;
        //beatTempo = beatTempo / 60f;
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
        } else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator"))
        {
            runwayDelay = Time.time - startTime;
        }
    }

    public float GetRunwayDelay()
    {
        return runwayDelay;
    }

}
