using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachine : MonoBehaviour
{
    private ParticleSystem particles;
    private BobaMiniGame mg;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        mg = (BobaMiniGame)MiniGameManager.GetMiniGame("Boba");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SmallFill()
    {
        mg.cup.gameObject.GetComponentInChildren<Ice>().SmallFill();
    }

    public void MediumFill()
    {
        mg.cup.gameObject.GetComponentInChildren<Ice>().MediumFill();
    }

    public void LargeFill()
    {
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
