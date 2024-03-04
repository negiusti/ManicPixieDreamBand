using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager[] gms;

    // Start is called before the first frame update
    void Start()
    {

            
            

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


    void RegisterSOLuaFuncs()
    {
        Lua.RegisterFunction(nameof(Calendar.CompleteCurrentEvent), this, SymbolExtensions.GetMethodInfo(() => Calendar.CompleteCurrentEvent()));
        Lua.RegisterFunction(nameof(MiniGameManager.StartMiniGame), this, SymbolExtensions.GetMethodInfo(() => MiniGameManager.StartMiniGame(string.Empty)));
        Lua.RegisterFunction(nameof(JamCoordinator.StartJam), this, SymbolExtensions.GetMethodInfo(() => JamCoordinator.StartJam(string.Empty)));
        Lua.RegisterFunction(nameof(JamCoordinator.EndJam), this, SymbolExtensions.GetMethodInfo(() => JamCoordinator.EndJam()));
    }

    void UnregisterSOLuaFuncs()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(Calendar.CompleteCurrentEvent));
        Lua.UnregisterFunction(nameof(MiniGameManager.StartMiniGame));
        Lua.UnregisterFunction(nameof(JamCoordinator.StartJam));
        Lua.UnregisterFunction(nameof(JamCoordinator.EndJam));
        //}
    }

    void SubscribeToEvents()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().ConvoCompleted += Calendar.OnConversationComplete;
    }

    void UnsubscribeFromEvents()
    {
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().ConvoCompleted -= Calendar.OnConversationComplete;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        SaveData();
    }

    private void LoadData()
    {
        MainCharacterState.Load();
        Calendar.Load();
        InventoryManager.LoadInventories();
        ConversationJson.LoadFromJson().WaitForCompletion();
        BandJson.LoadFromJson().WaitForCompletion();
    }

    private void SaveData()
    {
        MainCharacterState.Save();
        Calendar.Save();
        InventoryManager.SaveInventories();
    }

    // Update is called once per frame
    void Update()
    {
        //Steamworks.SteamClient.RunCallbacks();
    }

    public void Quit()
    {
        SaveData();
        UnregisterSOLuaFuncs();
        UnsubscribeFromEvents();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
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
            RegisterSOLuaFuncs();
            SubscribeToEvents();
            LoadData();
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
