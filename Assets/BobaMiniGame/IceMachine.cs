using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachine : MonoBehaviour
{
    private ParticleSystem particles;
    private BobaMiniGame mg;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SmallFill()
    {
        audioSource.Play();
        mg.cup.gameObject.GetComponentInChildren<Ice>().SmallFill();
    }

    public void MediumFill()
    {
        audioSource.Play();
        mg.cup.gameObject.GetComponentInChildren<Ice>().MediumFill();
    }

    public void LargeFill()
    {
        audioSource.Play();
        mg.cup.gameObject.GetComponentInChildren<Ice>().LargeFill();
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
