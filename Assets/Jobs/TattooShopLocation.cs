using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class TattooShopLocation: MonoBehaviour
{
    private string closedConvo = "TattooShop_Closed";
    private string workConvo = "TattooShop_Work1";
    private bool convoDone;
    private bool hereForWork;

    // Start is called before the first frame update
    void Start()
    {
        hereForWork = JobSystem.CurrentJob().Equals(JobSystem.PunkJob.Tattoo) && Calendar.GetCurrentEvent() is JobEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!convoDone && !DialogueManager.IsConversationActive)
        {
            if (hereForWork)
                DialogueManager.StartConversation(workConvo);
            else
                DialogueManager.StartConversation(closedConvo);
            convoDone = true;
        }
    }
}
