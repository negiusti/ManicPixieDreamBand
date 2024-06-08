using UnityEngine;

public class StarControllerScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool canHit;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode) && canHit)
        {
            this.gameObject.GetComponent<StarMoverScript>().enabled = false;
            animator.Play("DeathStar");
        }
    }

    public void KillStar()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            canHit = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            canHit = false;
    }
}
