using System;
using UnityEngine;

public class BassMiniGame : MiniGame
{
    private bool isActive;
    private GameObject mainCamera;
    private Camera mgCamera;
    private BlackScreen blackScreen;
    private bool closingInProgress;

    // Use this for initialization
    void Start()
    {
        mgCamera = GetComponentInChildren<Camera>(true);
        blackScreen = GetComponentInChildren<BlackScreen>(true);
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
            blackScreen.Unfade();
        }
        MiniGameManager.PrepMiniGame();
        //JamCoordinator.SwitchToJamCamera();
        isActive = true;
        closingInProgress = false;
        EnableAllChildren();
    }

    public void StartBassMiniGameWithBand(bool repeat)
    {
        // TODO: repeat mini game if not gig
        OpenMiniGame();
        JamCoordinator.StartJam("LEMON BOY");
    }

    public void Fade()
    {
        if (closingInProgress)
            return;
        closingInProgress = true;
        blackScreen.Fade();
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
