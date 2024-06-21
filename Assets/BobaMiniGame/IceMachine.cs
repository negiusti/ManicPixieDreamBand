using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachine : MonoBehaviour
{
    private ParticleSystem particles;
    public GameObject ice;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        if (ice.transform.localPosition.y >= -1.58f) // max;
            return;
        ice.transform.Translate(Vector3.up * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        particles.Play();
    }

    private void OnMouseUp()
    {
        particles.Stop();
    }
}
