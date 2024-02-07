using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager[] gms;

    // Start is called before the first frame update
    void Start()
    {
        gms = FindObjectsOfType<GameManager>();

        // If there is more than one instance, destroy the current object
        if (gms.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            LoadData();
            SceneManager.activeSceneChanged += ChangedActiveScene;

            //try
            //{
            //    Steamworks.SteamClient.Init(2772090);
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogError("Could not initialize steam client: + " + e.ToString());
            //    // Something went wrong - it's one of these:
            //    //
            //    //     Steam is closed?
            //    //     Can't find steam_api dll?
            //    //     Don't have permission to play app?
            //    //
            //}
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        SaveData();
    }

    private void LoadData()
    {
        MainCharacterState.Load();
    }

    private void SaveData()
    {
        MainCharacterState.Save();
    }

    // Update is called once per frame
    void Update()
    {
        //Steamworks.SteamClient.RunCallbacks();
    }

    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            SaveData();
    #else
            Application.Quit();
    #endif
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnDestroy()
    {
        //if (gms.Length == 1)
        //    Steamworks.SteamClient.Shutdown();
        // SAVE THE CURRENT STATE OF EVERYTHING
        // phone: contacts, messages, money
        // conversations
        // worn outfits
        // unlocked objects
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
