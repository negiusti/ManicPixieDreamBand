using UnityEngine.SceneManagement;
using UnityEngine;
using PixelCrushers.DialogueSystem;

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
        Lua.RegisterFunction(nameof(Characters.Emote), this, SymbolExtensions.GetMethodInfo(() => Characters.Emote(string.Empty, string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddToMCInventory), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddToMCInventory(string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddPerishableItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddPerishableItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.RemoveItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.RemoveItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.RemovePerishableItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.RemovePerishableItem(string.Empty)));
        Lua.RegisterFunction(nameof(MainCharacterState.HasChangedOutfitToday), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.HasChangedOutfitToday()));
        Lua.RegisterFunction(nameof(MainCharacterState.CurrentBankBalance), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.CurrentBankBalance()));
        Lua.RegisterFunction(nameof(JobSystem.SetCurrentJob), this, SymbolExtensions.GetMethodInfo(() => JobSystem.SetCurrentJob(string.Empty)));
        Lua.RegisterFunction(nameof(QuestManager.CompleteCurrentQuest), this, SymbolExtensions.GetMethodInfo(() => QuestManager.CompleteCurrentQuest()));
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
        Lua.UnregisterFunction(nameof(Characters.Emote));
        Lua.UnregisterFunction(nameof(InventoryManager.AddToMCInventory));
        Lua.UnregisterFunction(nameof(InventoryManager.AddItem));
        Lua.UnregisterFunction(nameof(InventoryManager.AddPerishableItem));
        Lua.UnregisterFunction(nameof(InventoryManager.RemoveItem));
        Lua.UnregisterFunction(nameof(InventoryManager.RemovePerishableItem));
        Lua.UnregisterFunction(nameof(MainCharacterState.HasChangedOutfitToday));
        Lua.UnregisterFunction(nameof(MainCharacterState.CurrentBankBalance));
        Lua.UnregisterFunction(nameof(JobSystem.SetCurrentJob));
        Lua.UnregisterFunction(nameof(QuestManager.CompleteCurrentQuest));
        //}
    }

    void SubscribeToEvents()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        SceneManager.activeSceneChanged += Characters.RefreshCharactersCache;
        DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().ConvoCompleted += Calendar.OnConversationComplete;
    }

    void UnsubscribeFromEvents()
    {
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        SceneManager.activeSceneChanged -= Characters.RefreshCharactersCache;
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
        ConversationJson.LoadQuestsFromJson().WaitForCompletion();
        BandJson.LoadFromJson().WaitForCompletion();
        JobSystem.Load();
        QuestManager.Load();
    }

    private void SaveData()
    {
        MainCharacterState.Save();
        Calendar.Save();
        InventoryManager.SaveInventories();
        JobSystem.Save();
        QuestManager.Save();
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
        SaveData();

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
