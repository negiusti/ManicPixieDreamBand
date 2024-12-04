using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sk8FilmerMiniGame : MonoBehaviour
{
    //private BlackScreen blackScreen;
    private CameraShaker mgCam;
    private bool isActive;
    public bool startNow;
    public Character Rex;
    public MainCharacter mc;
    //public float maxX;
    //public float maxY = 7f;
    //public float minX;
    //public float minY = -9.3f;

    private void Start()
    {
        //blackScreen = GetComponentInChildren<BlackScreen>(true);
        mgCam = GetComponentInChildren<CameraShaker>(true);
    }

    private IEnumerator CloseMinigameAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Fade();
        yield return null;
    }

    //private IEnumerator ShakeCameraEvery(float seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    mgCam.CameraShake();
    //    yield return null;
    //}

    public void OpenMiniGame()
    {
        // TODO: this may be necessary for other mingames!!!
        //if (!MiniGameManager.InteractionEnabled())
        //    return;

        // Opening up the minigame
        //blackScreen.Unfade();
        //MiniGameManager.PrepMiniGame();
        isActive = true;
        Rex.GetComponent<NPCMovement>().SkateTo(480f);
        StartCoroutine(CloseMinigameAfterTime(30f));
    }

    public void CloseMiniGame()
    {
        // CHANGE SCENE BACK!!!!!
        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    public void Fade()
    {
        //if (blackScreen != null)
        //    blackScreen.Fade();
        //else
            CloseMiniGame();
    }

    private void Update()
    {
        if (startNow && !isActive)
        {
            OpenMiniGame();
        }
    }
}
