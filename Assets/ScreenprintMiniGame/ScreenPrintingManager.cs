using UnityEngine;
using TMPro;

public class ScreenPrintingManager : MonoBehaviour
{
    public int score;

    public float timer = 30f; // How much time the player has

    private ScreenMoverScript screen;

    public TextMeshPro scoreText;
    public TextMeshPro timerText;

    private void Start()
    {
        screen = GetComponentInChildren<ScreenMoverScript>();
    }

    private void Update()
    {
        // Count down if the timer is not at 0
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }

        // Set the timer text to the nearest integer
        timerText.text = Mathf.RoundToInt(timer).ToString();

        if (timer <= 0)
        {
            screen.moveScreen = false;

            screen.result.text = "Time's Up!";
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
