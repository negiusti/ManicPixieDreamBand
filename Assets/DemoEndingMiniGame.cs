using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEndingMiniGame : MiniGame
{
    private GameObject mainCamera;
    private BlackScreen blackScreen;
    private bool isActive;
    private Coroutine cr;

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

    private void OnDestroy()
    {
        if (cr != null)
            StopCoroutine(cr);
    }

    private IEnumerator ThanksForPlaying()
    {
        blackScreen.Unfade();
        yield return new WaitForSeconds(5f);
        blackScreen.Fade();
        yield return null;
    }

    public override void OpenMiniGame()
    {
        // Opening up the minigame

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
        cr = StartCoroutine(ThanksForPlaying());
    }

    public override void CloseMiniGame()
    {
        mainCamera.SetActive(true);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
        SceneChanger.Instance.ChangeScene("Splash");
    }

    public void Fade()
    {
        if (blackScreen != null)
            blackScreen.Fade();
        else
            CloseMiniGame();
    }
}
