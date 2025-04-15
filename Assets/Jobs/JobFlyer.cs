using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class JobFlyer : MonoBehaviour, IPointerDownHandler
{
    public CorkboardMiniGame corkboard;
    public JobSystem.PunkJob job;
    private SpriteResolver spriteResolver;
    private Animator animator;
    private bool taken;

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (animator == null)
            Start();
        taken = JobSystem.CurrentJob().Equals(job);
        if (animator != null)
        {
            animator.SetBool("InRange", !taken);
        }
        if (taken)
        {
            if (GetComponent<BiggerWhenHovered>() != null)
            {
                Destroy(gameObject.GetComponent<BiggerWhenHovered>());
            }
        } else if (!taken)
        {
            if (GetComponent<BiggerWhenHovered>() == null)
            {
                gameObject.AddComponent<BiggerWhenHovered>();
            }
        }
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
        Phone.Instance.ReceiveMsg("TXT/" + job.ToString() + " Boss/Hire", true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}
