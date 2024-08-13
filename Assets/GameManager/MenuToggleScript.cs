using PixelCrushers.DialogueSystem;
using UnityEngine;

public class MenuToggleScript : MonoBehaviour
{
    public GameObject menuToToggle;
    private Camera prevCamera;
    //private SpriteRenderer menuBackground;

    private void Start()
    {
        //menuBackground = this.GetComponentInChildren<SpriteRenderer>();
        prevCamera = Camera.main;
        DisableMenu();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        if (Phone.Instance != null && SceneChanger.Instance.GetCurrentScene() != "Character_Editor")
            Phone.Instance.gameObject.SetActive(true);
        DialogueManager.Unpause();
        menuToToggle.SetActive(false);
        prevCamera.enabled = true;
        Time.timeScale = 1f;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources)
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
        DialogueManager.Pause();
        prevCamera = Camera.main;
        prevCamera.enabled = false;
        menuToToggle.SetActive(true);
        Time.timeScale = 0f;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources)
        {
            audio.Pause();
        }
    }

    private bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            !MiniGameManager.GetMiniGame("Calibration").IsMiniGameActive()&&
            (DialogueManager.Instance == null || !DialogueManager.Instance.GetComponent<CustomDialogueScript>().IsTxtConvoActive());
    }

}
