using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ImprovMiniGame : MiniGame
{
    public List<AudioClip> pentatonicAudioClips;
    private bool isActive;
    private GameObject mainCamera;
    private Camera mgCamera;
    private BlackScreen blackScreen;
    private bool closingInProgress;
    private AudioSource audioSource;
    public AudioSource jjRiff;
    private Player player;

    // Use this for initialization
    void Start()
    {
        mgCamera = GetComponentInChildren<Camera>(true);
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        audioSource = GetComponent<AudioSource>();
        player = ReInput.players.GetPlayer(0);
        DisableAllChildren();
    }

    private void OnDestroy()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Back") && !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen())
        {
            CloseMiniGame();
        }
        //if (Input.GetKeyDown(KeyCode.G) && !IsMiniGameActive())
        //    StartBassMiniGameWithJJ();
        if (jjRiff.time >= jjRiff.clip.length - 1)
        {
            CloseMiniGame();
        }
    }

    public override void OpenMiniGame()
    {
        GameManager.bgMusic.PauseAudio();
        if (mgCamera != null)
        {
            mainCamera = Camera.main.transform.gameObject;
            mainCamera.SetActive(false);
            blackScreen.Unfade();
        }
        MiniGameManager.PrepMiniGame();
        isActive = true;
        closingInProgress = false;
        EnableAllChildren();
    }

    public void StartBassMiniGameWithJJ()
    {
        OpenMiniGame();
        JamCoordinator.StartJam("JJ JAZZ");
    }

    public void PlayPentatonicNote(int clipIndex)
    {
        if (pentatonicAudioClips.Count < clipIndex)
            return;

        Debug.Log("Playing: " + pentatonicAudioClips[clipIndex].name);

        audioSource.clip = pentatonicAudioClips[clipIndex];
        audioSource.Play();
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
        if (!isActive)
            return;
        GameManager.bgMusic.UnpauseAudio();
        if (mgCamera != null)
        {
            mainCamera.SetActive(true);
        }
        isActive = false;
        JamCoordinator.EndJam();
        MiniGameManager.CleanUpMiniGame();
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }
}
