using UnityEngine;

public class SleepingScreenMiniGame : MiniGame
{
    private GameObject mainCamera;
    private GameObject mgCamera;
    private bool isActive;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mgCamera = GetComponentInChildren<Camera>().gameObject;
        DisableAllChildren();
        //OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        // Opening up the minigame

        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        mgCamera.transform.position = new Vector3(transform.position.x, mgCamera.transform.position.y, mgCamera.transform.position.z);
        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
        animator.Play("SleepingLoadingScreen_Anim", -1, 0f);
        Calendar.Sleep();
        Characters.MainCharacter().transform.position = new Vector3(transform.position.x, Characters.MainCharacter().transform.position.y, Characters.MainCharacter().transform.position.z);
    }

    public override void CloseMiniGame()
    {

        if (mainCamera == null)
            return;

        mainCamera.SetActive(true);
        mainCamera.transform.position = new Vector3(mgCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

        DisableAllChildren();

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

}
