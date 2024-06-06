using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkboardMiniGame : MiniGame
{
    private GameObject mainCamera;
    public GameObject blackScreen;
    private bool isActive;

    private void Start()
    {
        DisableAllChildren();
        //OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        // Opening up the minigame

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;

        blackScreen.SetActive(false);
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

    }
}
