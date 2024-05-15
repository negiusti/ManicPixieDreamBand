using PixelCrushers.DialogueSystem;
using UnityEngine;

public class InteractHintScript : MonoBehaviour
{
    private bool inRange;
    public string sceneToTrigger;
    public KeyCode keyToTrigger;
    private SceneChanger sc;
    public GameObject objToEnable;
    private GameManager gm;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    //private Vector3 originalScale;
    //private bool isInsideTrigger = false;
    //public float scaleFactor = 0.1f; // 10% scale factor change
    //public float pulseSpeed = 2.0f; // Adjust the speed of the pulse
    public string animationName;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        sc = FindObjectOfType<SceneChanger>();
        if (objToEnable != null)
            objToEnable.SetActive(false);
        gm = GameManager.Instance;
        sc = gm.gameObject.GetComponent<SceneChanger>();
        animator = GetComponent<Animator>();
        if (animationName == null)
        {
            Debug.LogError("No animation given");
        }
        animator.enabled = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKey(keyToTrigger) && InteractionEnabled())
        {
            if (objToEnable != null)
                objToEnable.SetActive(true);
            if (sceneToTrigger != null && sceneToTrigger.Length > 0)
                sc.ChangeScene(sceneToTrigger);
        }
    }

    private bool InteractionEnabled()
    {
        return !Characters.MainCharacter().gameObject.GetComponent<Movement>().IsSkating() &&
            !SceneChanger.Instance.IsLoadingScreenOpen() &&
            !DialogueManager.IsConversationActive;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player") && InteractionEnabled())
    //    {
    //        inRange = true;
    //        animator.enabled = true;
    //        animator.Play(animationName);
    //    }
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && InteractionEnabled())
        {
            inRange = true;
            animator.enabled = true;
            animator.Play(animationName);
        }
        else
        {
            inRange = false;
            spriteRenderer.color = Color.white;
            animator.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            spriteRenderer.color = Color.white;
            animator.enabled = false;
        }
    }
}
