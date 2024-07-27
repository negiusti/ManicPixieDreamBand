using UnityEngine;
using UnityEngine.U2D.Animation;

public class JobFlyer : MonoBehaviour
{
    public CorkboardMiniGame corkboard;
    public JobSystem.PunkJob job;
    private SpriteResolver spriteResolver;
    private bool taken;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
    }

    private void OnEnable()
    {
        taken = JobSystem.CurrentJob().Equals(job);
        if (spriteResolver == null)
            Start();

        if (taken)
        {
            Debug.Log("Current job: " + JobSystem.CurrentJob().ToString() + " flyer job: " + job.ToString());
            Debug.Log("spriteResolver.SetCategoryAndLabel(Flyer, Close);");
            spriteResolver.SetCategoryAndLabel("Flyer", "Close");
        } else
        {
            Debug.Log("Current job: " + JobSystem.CurrentJob().ToString() + " flyer job: " + job.ToString());
            Debug.Log("spriteResolver.SetCategoryAndLabel(Flyer, Open);");
            spriteResolver.SetCategoryAndLabel("Flyer", "Open");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (taken)
            return;
        corkboard.CloseMiniGame();
        Phone.Instance.Unlock();
        Phone.Instance.ReceiveMsg("TXT/" + job.ToString() + " Boss/Hire");
        Phone.Instance.OpenTxtConvoWith(job.ToString() + " Boss");
    }
}
