using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CalibrationMiniGame : MiniGame
{
    private bool isActive;
    private GameObject mainCamera;
    public Timer timer;

    private void Start()
    {
        timer = GetComponentInChildren<Timer>(true);
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    //private void Awake()
    //{
    //    OpenMiniGame();
    //}

    public override void OpenMiniGame()
    {
        // Opening up the minigame
        //MiniGameManager.PrepMiniGame();
        // Let's not prep minigames started from GM menu

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();
        GameManager.Instance.PauseBGMusic(); // Need to be able to hear the metronome
        isActive = true;
        timer.gameObject.SetActive(false);
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        DisableAllChildren();
        GameManager.Instance.UnpauseBGMusic();
        isActive = false;
        //MiniGameManager.CleanUpMiniGame();
    }

    private void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Backspace))
            CloseMiniGame();
    }
}
