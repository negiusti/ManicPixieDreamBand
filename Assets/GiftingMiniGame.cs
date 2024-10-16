using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftingMiniGame : MiniGame
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
        EnableAllChildren();
        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public void OpenMiniGame(string npcName)
    {
        EnableAllChildren();
        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public void RegisterNPCReaction()
    {

    }

    public override void CloseMiniGame()
    {

        DisableAllChildren();
        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }
}
