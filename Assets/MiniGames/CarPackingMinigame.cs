using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPackingMinigame : MiniGame
{
    public TrunkDoor trunkDoor;

    private Camera mainCamera;
    private bool isActive;

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        mainCamera = Camera.main;

        mainCamera.enabled = false;

        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public override void CloseMiniGame()
    {
        mainCamera.enabled = true;

        isActive = false;
    }

    public void Win()
    {
        trunkDoor.CloseTrunk();
    }
}
