using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TextMeshPro timerText; 
    public int timeInSeconds;
    private int timeRemaining;
    private Animator animator;

    void Start()
    {
        Reset();
        timerText = GetComponent<TextMeshPro>();
        animator = GetComponent<Animator>();
        StartCoroutine(Countdown()); 
    }

    private void Update()
    {
        animator.SetBool("TimerLow", timeRemaining > 0 && timeRemaining < 10);
        animator.SetBool("TimerDone", timeRemaining <= 0);
    }

    public void Reset()
    {
        timeRemaining = timeInSeconds;
        timerText.text = timeRemaining.ToString();
    }

    public void Restart()
    {
        if (IsRunning())
            return;
        Reset();
        StartCoroutine(Countdown());
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    public bool IsRunning()
    {
        return timeRemaining > 0 && timeRemaining <= timeInSeconds;
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1;
            timerText.text = timeRemaining.ToString(); // Update the text to show the remaining time
        }

        // Optionally, add any actions to be performed when the timer reaches zero
        TimerEnded();
    }

    private void TimerEnded()
    {
        // Actions to perform when the timer reaches zero
        Debug.Log("Timer has ended!");
    }
}
