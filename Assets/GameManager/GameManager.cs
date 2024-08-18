using UnityEngine.SceneManagement;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager[] gms;
    public static BGMusicController bgMusic;

    // Start is called before the first frame update
    void Start()
    {

        RegisterSOLuaFuncs();
        SubscribeToEvents();
        LoadData();


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
        Lua.RegisterFunction(nameof(Characters.MoveYPos), this, SymbolExtensions.GetMethodInfo(() => Characters.MoveYPos(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.EnableCharacter), this, SymbolExtensions.GetMethodInfo(() => Characters.EnableCharacter(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.DisableCharacter), this, SymbolExtensions.GetMethodInfo(() => Characters.DisableCharacter(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.DisableDialogueTrigger), this, SymbolExtensions.GetMethodInfo(() => Characters.DisableDialogueTrigger(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.NPCWalkTo), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCWalkTo(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.NPCSkateTo), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCSkateTo(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.NPCFaceRight), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCFaceRight(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.NPCFaceLeft), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCFaceLeft(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddToMCInventory), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddToMCInventory(string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddPerishableItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddPerishableItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.RemoveItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.RemoveItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.RemovePerishableItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.RemovePerishableItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.GetNumInPockets), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.GetNumInPockets(string.Empty)));
        Lua.RegisterFunction(nameof(MainCharacterState.HasChangedOutfitToday), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.HasChangedOutfitToday()));
        Lua.RegisterFunction(nameof(MainCharacterState.CurrentBankBalance), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.CurrentBankBalance()));
        Lua.RegisterFunction(nameof(MainCharacterState.ModifyBankBalance), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.ModifyBankBalance((double)0)));
        Lua.RegisterFunction(nameof(MainCharacterState.UnlockPhoto), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.UnlockPhoto(string.Empty)));
        Lua.RegisterFunction(nameof(MainCharacterState.SetFlag), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.SetFlag(string.Empty, false)));
        Lua.RegisterFunction(nameof(MainCharacterState.CheckFlag), this, SymbolExtensions.GetMethodInfo(() => MainCharacterState.CheckFlag(string.Empty)));
        Lua.RegisterFunction(nameof(JobSystem.SetCurrentJob), this, SymbolExtensions.GetMethodInfo(() => JobSystem.SetCurrentJob(string.Empty)));
        Lua.RegisterFunction(nameof(JobSystem.CurrentJobString), this, SymbolExtensions.GetMethodInfo(() => JobSystem.CurrentJobString()));
        Lua.RegisterFunction(nameof(QuestManager.CompleteCurrentQuest), this, SymbolExtensions.GetMethodInfo(() => QuestManager.CompleteCurrentQuest()));
        Lua.RegisterFunction(nameof(RomanceManager.ChangeRelationshipScore), this, SymbolExtensions.GetMethodInfo(() => RomanceManager.ChangeRelationshipScore(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Calendar.SchedulePlotEvent), this, SymbolExtensions.GetMethodInfo(() => Calendar.SchedulePlotEvent(string.Empty, string.Empty, string.Empty, false, (double)0)));
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
        Lua.UnregisterFunction(nameof(Characters.MoveYPos));
        Lua.UnregisterFunction(nameof(Characters.EnableCharacter));
        Lua.UnregisterFunction(nameof(Characters.DisableCharacter));
        Lua.UnregisterFunction(nameof(Characters.DisableDialogueTrigger));
        Lua.UnregisterFunction(nameof(Characters.NPCWalkTo));
        Lua.UnregisterFunction(nameof(Characters.NPCSkateTo));
        Lua.UnregisterFunction(nameof(Characters.NPCFaceLeft));
        Lua.UnregisterFunction(nameof(Characters.NPCFaceRight));
        Lua.UnregisterFunction(nameof(InventoryManager.AddToMCInventory));
        Lua.UnregisterFunction(nameof(InventoryManager.AddItem));
        Lua.UnregisterFunction(nameof(InventoryManager.AddPerishableItem));
        Lua.UnregisterFunction(nameof(InventoryManager.RemoveItem));
        Lua.UnregisterFunction(nameof(InventoryManager.RemovePerishableItem));
        Lua.UnregisterFunction(nameof(InventoryManager.GetNumInPockets));
        Lua.UnregisterFunction(nameof(MainCharacterState.HasChangedOutfitToday));
        Lua.UnregisterFunction(nameof(MainCharacterState.CurrentBankBalance));
        Lua.UnregisterFunction(nameof(MainCharacterState.ModifyBankBalance));
        Lua.UnregisterFunction(nameof(MainCharacterState.UnlockPhoto));
        Lua.UnregisterFunction(nameof(MainCharacterState.SetFlag));
        Lua.UnregisterFunction(nameof(MainCharacterState.CheckFlag));
        Lua.UnregisterFunction(nameof(JobSystem.SetCurrentJob));
        Lua.UnregisterFunction(nameof(JobSystem.CurrentJobString));
        Lua.UnregisterFunction(nameof(QuestManager.CompleteCurrentQuest));
        Lua.UnregisterFunction(nameof(RomanceManager.ChangeRelationshipScore));
        Lua.UnregisterFunction(nameof(Calendar.SchedulePlotEvent));
        //}
    }

    void SubscribeToEvents()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        SceneManager.activeSceneChanged += Characters.RefreshCharactersCache;
    }

    void UnsubscribeFromEvents()
    {
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        SceneManager.activeSceneChanged -= Characters.RefreshCharactersCache;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        SaveData();
    }

    public void RefreshGameState()
    {
        SaveSystem.DeleteSaveData();
        MainCharacterState.Load();
        Calendar.Load();
        InventoryManager.ResetInventories();
        JobSystem.Load();
        QuestManager.Load();
        RomanceManager.Load();
        Tutorial.Load();
        if (Phone.Instance != null)
            Phone.Instance.Reset();
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.GetComponent<CustomDialogueScript>().Reset();
            DialogueManager.StopAllConversations();
        }
    }

    private void LoadData()
    {
        MainCharacterState.Load();
        Calendar.Load();
        InventoryManager.LoadInventories();
        ConversationJson.LoadFromJson().WaitForCompletion();
        ConversationJson.LoadQuestsFromJson().WaitForCompletion();
        ConversationJson.LoadRomancesFromJson().WaitForCompletion();
        BandJson.LoadFromJson().WaitForCompletion();
        JobSystem.Load();
        QuestManager.Load();
        RomanceManager.Load();
        Tutorial.Load();
    }

    private void SaveData()
    {
        MainCharacterState.Save();
        Calendar.Save();
        InventoryManager.SaveInventories();
        JobSystem.Save();
        QuestManager.Save();
        RomanceManager.Save();
        Tutorial.Save();
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
            bgMusic = GetComponent<BGMusicController>();
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
