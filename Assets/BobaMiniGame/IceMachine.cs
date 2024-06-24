using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachine : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        particles.Play();
    }

    public void Off()
    {
        particles.Stop();
    }
}
