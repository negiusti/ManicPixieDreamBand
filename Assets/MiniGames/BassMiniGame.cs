using System;
using UnityEngine;

public class BassMiniGame : MiniGame
{
    private bool isActive;
    private GameObject mainCamera;
    private Camera mgCamera;

    // Use this for initialization
    void Start()
    {
        mgCamera = GetComponentInChildren<Camera>(true);
        DisableAllChildren();
    }

    private void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CloseMiniGame();
        }
    }

    public override void OpenMiniGame()
    {
        if (mgCamera != null)
        {
            mainCamera = Camera.main.transform.gameObject;
            mainCamera.SetActive(false);
        }
        MiniGameManager.PrepMiniGame();
        //JamCoordinator.SwitchToJamCamera();
        isActive = true;
        EnableAllChildren();
    }

    public void StartBassMiniGameWithBand(bool repeat)
    {
        // TODO: repeat mini game if not gig
        OpenMiniGame();
        JamCoordinator.StartJam("LEMON BOY");
    }

    public override void CloseMiniGame()
    {
        if (mgCamera != null)
        {
            mainCamera.SetActive(true);
        }
        isActive = false;
        //JamCoordinator.SwitchToMainCamera();
        JamCoordinator.EndJam();
        MiniGameManager.CleanUpMiniGame();
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }
}
