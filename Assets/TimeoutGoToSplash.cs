using UnityEngine.InputSystem;
using UnityEngine;

public class TimeoutGoToSplash : MonoBehaviour
{
    private SceneChanger sc;
    private float countdownTimer;
    public float timeLimitSeconds = 180f;
    public bool arcadeMode;
    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<SceneChanger>();
        countdownTimer = timeLimitSeconds;
    }


    void Update()
    {
        // Check for mouse/cursor movement
        if (Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            // Reset the countdown timer
            ResetTimer();
        }
        if (Input.anyKeyDown)
        {
            ResetTimer();
        }

        // Update and check the countdown timer
        UpdateTimer();
    }

    void UpdateTimer()
    {
        // Update the countdown timer
        countdownTimer -= Time.deltaTime;
        //Debug.Log("Countdown: " + countdownTimer);
        // Check if the countdown timer has reached zero
        if (countdownTimer <= 0f)
        {
            // Do something when the timer reaches zero
            Debug.Log("Countdown timer reached zero!");
            if (arcadeMode)
                sc.ChangeScene("SplashArcade");
            else
                sc.ChangeScene("Splash");

            // Reset the timer for the next interval
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        // Reset the countdown timer to 3 minutes
        countdownTimer = timeLimitSeconds;
    }
}
