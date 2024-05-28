using UnityEngine;

public class TattooMachine : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        animator = GetComponent<Animator>();
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
        if (Input.GetMouseButton(0)) // 0 is the left mouse button
        {
            animator.SetBool("Buzzing", true);
        }
        else
        {
            animator.SetBool("Buzzing", false);
        }
    }
}