using UnityEngine;

public class StarMoverScript : MonoBehaviour
{
    public float initialSpeed = 5f; // Starting speed
    public float acceleration = 0.5f; // Rate of speed increase per second
    private Animator animator;

    private float currentSpeed;
    private bool dead;

    void Start()
    {
        // Set the initial speed when the script starts
        currentSpeed = initialSpeed;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Update the speed based on acceleration
        currentSpeed += acceleration * Time.deltaTime;

        // Move the GameObject upward on the Y-axis
        transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);
        if (transform.localPosition.y > 34f && !dead)
        {
            animator.Play("DeathStar2");
            dead = true;
        }
        if (transform.localPosition.y > 50f && dead)
        {
            Destroy(gameObject);
        }

    }

}