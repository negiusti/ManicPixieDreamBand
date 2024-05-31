using UnityEngine;

public class CarPackingMiniGame : MiniGame
{
    public TrunkDoor trunkDoor;

    private GameObject mainCamera;
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
        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public override void CloseMiniGame()
    {
        DisableAllChildren();

        mainCamera.SetActive(true);

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    public void Win()
    {
        trunkDoor.CloseTrunk();
    }
}
