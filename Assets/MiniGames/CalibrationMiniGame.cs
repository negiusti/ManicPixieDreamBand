using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationMiniGame : MiniGame
{
    private bool isActive;

    private void Start()
    {
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        // Opening up the minigame

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public override void CloseMiniGame()
    {

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    private void Update()
    {

    }
}
