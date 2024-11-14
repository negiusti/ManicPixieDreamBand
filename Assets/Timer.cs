using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    public TextMeshPro timerText; 
    public int timeInSeconds;
    private int timeRemaining;
    private Animator animator;
    private Coroutine coroutine;

    public int TimeRemaining()
    {
        return timeRemaining;
    }

    void Start()
    {
        timerText = GetComponent<TextMeshPro>();
        animator = GetComponent<Animator>();
        Reset();
    }

    private void Update()
    {
        animator.SetBool("TimerLow", timeRemaining > 0 && timeRemaining < 10);
        animator.SetBool("TimerDone", timeRemaining <= 0);
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = null;
    }

    private void OnEnable()
    {
        if (animator == null)
            Start();
        if (coroutine == null)
            coroutine = StartCoroutine(Countdown());
    }

    public void Reset()
    {
        timeRemaining = timeInSeconds;
        timerText.text = timeRemaining.ToString();
        animator.SetBool("TimerLow", false);
        animator.SetBool("TimerDone", false);
    }

    public void Restart()
    {
        if (IsRunning())
            return;
        Reset();
        if (coroutine == null)
            coroutine = StartCoroutine(Countdown());
    }

    public void StartTimer()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(Countdown());
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
        coroutine = null;
    }
}
