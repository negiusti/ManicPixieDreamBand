using PixelCrushers.DialogueSystem;
using UnityEngine;

public class BobaShopLocation: MonoBehaviour
{
    //private string closedConvo = "BobaShop_Closed";
    private string workConvo = "BobaShop_Work1";
    private bool convoDone;
    private bool hereForWork;

    // Start is called before the first frame update
    void Start()
    {
        hereForWork = JobSystem.CurrentJob().Equals(JobSystem.PunkJob.Boba) && Calendar.GetCurrentEvent() is JobEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!convoDone && !DialogueManager.IsConversationActive)
        {
            if (hereForWork)
                DialogueManager.StartConversation(workConvo);
            //else
            //    DialogueManager.StartConversation(closedConvo);
            convoDone = true;
        }
    }
}
