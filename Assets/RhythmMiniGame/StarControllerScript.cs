using UnityEngine;

public class StarControllerScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool canHit;
    private Animator animator;
    private bool keepitmoving;

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
        if (keepitmoving)
        {
            transform.position -= new Vector3(0f, 3f * Time.deltaTime, 0f);
        }
    }

    public void KillStar()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            canHit = true;
        keepitmoving = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
            canHit = false;
    }
}
