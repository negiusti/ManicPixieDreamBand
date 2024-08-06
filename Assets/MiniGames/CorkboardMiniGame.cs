using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkboardMiniGame : MiniGame
{
    private GameObject mainCamera;
    private BlackScreen blackScreen;
    private bool isActive;

    private void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        DisableAllChildren();
        //OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        // TODO: this may be necessary for other mingames!!!
        if (!MiniGameManager.InteractionEnabled())
            return;

        // Opening up the minigame

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;

        blackScreen.Unfade();
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    public void Fade()
    {
        if (blackScreen != null)
            blackScreen.Fade();
        else
            CloseMiniGame();
    }

    private void Update()
    {

    }
}
