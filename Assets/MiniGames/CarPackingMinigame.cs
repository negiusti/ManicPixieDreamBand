using UnityEngine;

public class CarPackingMiniGame : MiniGame
{
    public TrunkDoor trunkDoor;

    private Camera mainCamera;
    private bool isActive;

    private void Start()
    {
        //DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        mainCamera = Camera.main;

        EnableAllChildren();

        mainCamera.enabled = false;

        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public override void CloseMiniGame()
    {
        mainCamera.enabled = true;

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    public void Win()
    {
        trunkDoor.CloseTrunk();
    }
}
