using UnityEngine;
using Rewired;

public class StarControllerScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool canHit;
    private Animator animator;
    private bool keepitmoving;
    private StarSpawnerScript starSpawner;
    private SpriteRenderer spriteRen;
    bool hit;
    private Player player;
    private string inputName;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
        starSpawner = GetComponentInParent<StarSpawnerScript>(true);
        hit = false;
        player = ReInput.players.GetPlayer(0);
        switch (keyCode)
        {
            case KeyCode.Alpha1:
                inputName = "E string";
                break;
            case KeyCode.Alpha2:
                inputName = "A string";
                break;
            case KeyCode.Alpha3:
                inputName = "D string";
                break;
            case KeyCode.Alpha4:
                inputName = "G string";
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(keyCode) && canHit && !hit)
        if (player.GetButtonDown(inputName) && canHit && !hit)
        {
            this.gameObject.GetComponent<StarMoverScript>().enabled = false;
            animator.Play("DeathStar");
            starSpawner.HitNote();
            hit = true;
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
