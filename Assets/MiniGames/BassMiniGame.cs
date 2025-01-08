using System.Collections.Generic;
using UnityEngine;

public class BassMiniGame : MiniGame
{
    public List<AudioClip> oopsAudioClips;
    public List<AudioClip> goodAudioClips;
    private bool isActive;
    private GameObject mainCamera;
    private Camera mgCamera;
    private BlackScreen blackScreen;
    private bool closingInProgress;
    private AudioSource audioSource;
    private CameraShaker cameraShaker;
    private StarSpawnerScript starSpawner;
    public GameObject fret;
    public GameObject buttons;
    public GameObject buttonHints;
    public GameObject stars;
    private SongSelectionMenu songMenu;

    // Use this for initialization
    void Start()
    {
        mgCamera = GetComponentInChildren<Camera>(true);
        blackScreen = GetComponentInChildren<BlackScreen>(true);
        audioSource = GetComponent<AudioSource>();
        cameraShaker = mgCamera.GetComponent<CameraShaker>();
        starSpawner = GetComponentInChildren<StarSpawnerScript>(true);
        songMenu = GetComponentInChildren<SongSelectionMenu>(true);
        DisableAllChildren();
    }

    private void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen())
        {
            CloseMiniGame();
        }
    }

    public void SelectSong(string song)
    {
        fret.SetActive(true);
        stars.SetActive(true);
        buttons.SetActive(true);
        buttonHints.SetActive(true);
        starSpawner.StartSong(song);
        songMenu.gameObject.SetActive(false);
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
        //JamCoordinator.SwitchToJamCamera();
        isActive = true;
        closingInProgress = false;
        EnableAllChildren();
        fret.SetActive(false);
        stars.SetActive(false);
        buttons.SetActive(false);
        buttonHints.SetActive(false);
        songMenu.gameObject.SetActive(true);
    }

    public void StartBassMiniGameWithBand(bool repeat)
    {
        OpenMiniGame();
        JamCoordinator.StartJam("LEMON BOY");
    }

    public void StartJJBandMiniGame()
    {
        OpenMiniGame();
        SelectSong("JJ_JAZZ");
        JamCoordinator.StartJam("JJ BAND");
    }

    public void PlayBadSound()
    {
        if (oopsAudioClips.Count == 0 || cameraShaker == null)
            return;

        cameraShaker.CameraShake();
        int clipIndex = Random.Range(0, oopsAudioClips.Count);

        Debug.Log("Playing: " + oopsAudioClips[clipIndex].name);

        audioSource.clip = oopsAudioClips[clipIndex];
        audioSource.Play();
    }

    public void PlayGoodSound()
    {
        if (goodAudioClips.Count == 0)
            return;

        int clipIndex = Random.Range(0, goodAudioClips.Count);

        Debug.Log("Playing: " + goodAudioClips[clipIndex].name);

        audioSource.clip = goodAudioClips[clipIndex];
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
