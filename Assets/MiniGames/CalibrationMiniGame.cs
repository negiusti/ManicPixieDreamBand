using UnityEngine;

public class CalibrationMiniGame : MiniGame
{
    private bool isActive;
    private GameObject mainCamera;

    private void Start()
    {
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

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            CloseMiniGame();
    }
}
