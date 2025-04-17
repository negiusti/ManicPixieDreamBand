using UnityEngine;
using Rewired;

public class TattooMachine : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private AudioSource audioSource;
    private Player player;

    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing.");
        }
    }

    void Update()
    {
        // Follow the cursor position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set this to the distance you want the object to be from the camera
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

        // Check for mouse input and play the corresponding animation
        if (Input.GetMouseButton(0) || player.GetButton("Select Object Under Cursor")) // 0 is the left mouse button
        {
            animator.SetBool("Buzzing", true);
            audioSource.UnPause();
        }
        else
        {
            animator.SetBool("Buzzing", false);
            audioSource.Pause();
        }
    }
}