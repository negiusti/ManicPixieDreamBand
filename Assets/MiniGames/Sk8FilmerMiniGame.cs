using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sk8FilmerMiniGame : MiniGame
{
    private GameObject mainCamera;
    private BlackScreen blackScreen;
    private CameraShaker mgCam;
    private bool isActive;

    private void Start()
    {
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        mgCam = GetComponentInChildren<CameraShaker>(true);
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    private IEnumerator CloseMinigameAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Fade();
        yield return null;
    }

    private IEnumerator ShakeCameraEvery(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        mgCam.CameraShake();
        yield return null;
    }

    public override void OpenMiniGame()
    {
        // TODO: this may be necessary for other mingames!!!
        if (!MiniGameManager.InteractionEnabled())
            return;

        // Opening up the minigame
        mainCamera = Camera.main.transform.gameObject;
        EnableAllChildren();
        mainCamera.SetActive(false);
        blackScreen.Unfade();
        MiniGameManager.PrepMiniGame();
        isActive = true;
        Characters.NPCSkateBetween("JJ", -57.52, -2.24, 30);
        StartCoroutine(CloseMinigameAfterTime(30f));
        StartCoroutine(ShakeCameraEvery(2f));
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
        if (!isActive)
        {
            OpenMiniGame();
        }
    }
}
