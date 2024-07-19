using PixelCrushers.DialogueSystem;
using UnityEngine;

public class InteractHintScript : MonoBehaviour
{
    private bool inRange;
    public string sceneToTrigger;
    public MiniGame mgToTrigger;
    public KeyCode keyToTrigger;
    private SceneChanger sc;
    public GameObject objToEnable;
    private GameManager gm;
    private Animator animator;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) && InteractionEnabled())
        {
            if (mgToTrigger != null)
                mgToTrigger.OpenMiniGame();
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
            Phone.Instance.IsLocked() &&
            !DialogueManager.IsConversationActive &&
            !MiniGameManager.AnyActiveMiniGames();
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
            animator.SetBool("InRange", true);
        }
        else
        {
            inRange = false;
            animator.SetBool("InRange", false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            animator.SetBool("InRange", false);
        }
    }
}
