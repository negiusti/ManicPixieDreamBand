using UnityEngine;

public class StarControllerScript : MonoBehaviour
{
    public KeyCode keyCode;
    private bool canHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode) && canHit)
        {
            Destroy(gameObject);
        }
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
