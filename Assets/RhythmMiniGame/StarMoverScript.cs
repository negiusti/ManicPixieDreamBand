using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMoverScript : MonoBehaviour
{
    private float beatTempo = 4f;
    public bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
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
            }
        } else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
