using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class JobFlyer : MonoBehaviour
{
    public CorkboardMiniGame corkboard;
    public JobSystem.PunkJob job;
    private SpriteResolver spriteResolver;
    private Phone phone;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        phone = Phone.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        corkboard.CloseMiniGame();
        phone.Unlock();
        phone.ReceiveMsg("TXT_" + job.ToString() + " Boss_" + job.ToString() + "_Hire");
        phone.OpenTxtConvoWith(job.ToString() + " Boss");
    }
}
