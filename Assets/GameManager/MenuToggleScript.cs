using PixelCrushers.DialogueSystem;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using Rewired.UI.ControlMapper;
using System.Collections.Generic;

public class MenuToggleScript : MonoBehaviour
{
    public GameObject menuToToggle;
    private Camera prevCamera;
    private Player player;
    public Button SaveAndQuitButton;
    public Button SaveFilesButton;
    public GameObject WarningTxt;
    private HashSet<AudioSource> audioSourcesToUnPause;
    //private SpriteRenderer menuBackground;

    private void Start()
    {
        //menuBackground = this.GetComponentInChildren<SpriteRenderer>();
        prevCamera = Camera.main;
        player = ReInput.players.GetPlayer(0);
        audioSourcesToUnPause = new HashSet<AudioSource>();
        DisableMenu();
    }
    void Update()
    {
        if (player.GetButtonDown("Toggle Settings Menu"))
        {
            if (!InteractionEnabled())
                return;
            if (menuToToggle.activeSelf)
            {
                DisableMenu();
            }
            else
            {
                EnableMenu();
            }
        }
    }

    public bool IsMenuOpen()
    {
        return menuToToggle.activeSelf;
    }

    public void DisableMenu()
    {
        if (!menuToToggle.activeSelf)
            return;
        if (ControlMapper.current != null && ControlMapper.current.isOpen)
            return;
        if (Phone.Instance != null && SceneChanger.Instance.GetCurrentScene() != "Character_Editor")
            Phone.Instance.gameObject.SetActive(true);
        DialogueManager.Unpause();
        menuToToggle.SetActive(false);
        if (prevCamera != null)
            prevCamera.enabled = true;
        Time.timeScale = 1f;
        // AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSourcesToUnPause)
        {
            audio.UnPause();
        }
    }

    public void EnableMenu()
    {
        if (!InteractionEnabled())
            return;
        if (Phone.Instance != null)
            Phone.Instance.gameObject.SetActive(false);
        
        if (SaveAndQuitButton != null)
            SaveAndQuitButton.interactable = !DialogueManager.isConversationActive;
        if (SaveFilesButton != null)
            SaveFilesButton.interactable = !DialogueManager.isConversationActive;
        WarningTxt.SetActive(DialogueManager.isConversationActive);
        DialogueManager.Pause();
        prevCamera = Camera.main;
        prevCamera.enabled = false;
        menuToToggle.SetActive(true);
        Time.timeScale = 0f;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        audioSourcesToUnPause.Clear();
        foreach (AudioSource audio in audioSources)
        {
            if (audio.isPlaying) {
                audio.Pause();
                audioSourcesToUnPause.Add(audio);
            }
        }
    }

    private bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            !MiniGameManager.GetMiniGame("Calibration").IsMiniGameActive()&&
            (DialogueManager.Instance == null || !DialogueManager.Instance.GetComponent<CustomDialogueScript>().IsTxtConvoActive());
    }

}
