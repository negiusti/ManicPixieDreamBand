using UnityEngine.SceneManagement;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager[] gms;
    public static BGMusicController bgMusic;
    public static ButtonSoundEffects buttonSounds;
    public static MiscSoundEffects miscSoundEffects;

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
        Lua.RegisterFunction(nameof(MiniGameManager.StartGiftingMiniGame), this, SymbolExtensions.GetMethodInfo(() => MiniGameManager.StartGiftingMiniGame(string.Empty)));
        Lua.RegisterFunction(nameof(MiniGameManager.GetLastBassMiniGameScore), this, SymbolExtensions.GetMethodInfo(() => MiniGameManager.GetLastBassMiniGameScore()));
        Lua.RegisterFunction(nameof(MiniGameManager.GetLastScreenPrintingScore), this, SymbolExtensions.GetMethodInfo(() => MiniGameManager.GetLastScreenPrintingScore()));
        Lua.RegisterFunction(nameof(JamCoordinator.StartJam), this, SymbolExtensions.GetMethodInfo(() => JamCoordinator.StartJam(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(JamCoordinator.EndJam), this, SymbolExtensions.GetMethodInfo(() => JamCoordinator.EndJam()));
        Lua.RegisterFunction(nameof(Characters.UnlockEmoji), this, SymbolExtensions.GetMethodInfo(() => Characters.UnlockEmoji(string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(Characters.Emote), this, SymbolExtensions.GetMethodInfo(() => Characters.Emote(string.Empty, string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(Characters.Drink), this, SymbolExtensions.GetMethodInfo(() => Characters.Drink(string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(Characters.Shoot), this, SymbolExtensions.GetMethodInfo(() => Characters.Shoot(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.MoveYPos), this, SymbolExtensions.GetMethodInfo(() => Characters.MoveYPos(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.SetLayer), this, SymbolExtensions.GetMethodInfo(() => Characters.SetLayer(string.Empty, string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.EnableCharacter), this, SymbolExtensions.GetMethodInfo(() => Characters.EnableCharacter(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.DisableCharacter), this, SymbolExtensions.GetMethodInfo(() => Characters.DisableCharacter(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.DisableDialogueTrigger), this, SymbolExtensions.GetMethodInfo(() => Characters.DisableDialogueTrigger(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.NPCWalkTo), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCWalkTo(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.NPCSkateTo), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCSkateTo(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(Characters.NPCStopSkating), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCStopSkating(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.NPCKickFlip), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCKickFlip(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.NPCSkateBetween), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCSkateBetween(string.Empty, (double)0, (double)0, (double)0)));
        Lua.RegisterFunction(nameof(Characters.NPCFaceRight), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCFaceRight(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.NPCFaceLeft), this, SymbolExtensions.GetMethodInfo(() => Characters.NPCFaceLeft(string.Empty)));
        Lua.RegisterFunction(nameof(Characters.GetMostRecentGiftReaction), this, SymbolExtensions.GetMethodInfo(() => Characters.GetMostRecentGiftReaction()));
        Lua.RegisterFunction(nameof(Characters.RecordMostRecentGiftReaction), this, SymbolExtensions.GetMethodInfo(() => Characters.RecordMostRecentGiftReaction(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddToMCInventory), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddToMCInventory(string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.AddPerishableItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.AddPerishableItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.RemoveItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.RemoveItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.RemovePerishableItem), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.RemovePerishableItem(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.GetNumInPockets), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.GetNumInPockets(string.Empty)));
        Lua.RegisterFunction(nameof(InventoryManager.BuyPurchaseableTopOrOutfit), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.BuyPurchaseableTopOrOutfit()));
        Lua.RegisterFunction(nameof(InventoryManager.FindPurchaseableTopOrOutfitName), this, SymbolExtensions.GetMethodInfo(() => InventoryManager.FindPurchaseableTopOrOutfitName()));
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
        Lua.RegisterFunction(nameof(DailyRandoms.DrinkSpecial), this, SymbolExtensions.GetMethodInfo(() => DailyRandoms.DrinkSpecial()));
        Lua.RegisterFunction(nameof(AudioController.PauseBGMusic), this, SymbolExtensions.GetMethodInfo(() => AudioController.PauseBGMusic()));
        Lua.RegisterFunction(nameof(AudioController.UnpauseBGMusic), this, SymbolExtensions.GetMethodInfo(() => AudioController.UnpauseBGMusic()));
        Lua.RegisterFunction(nameof(AudioController.Clap), this, SymbolExtensions.GetMethodInfo(() => AudioController.Clap((double)0)));
        Lua.RegisterFunction(nameof(RefreshGameState), this, SymbolExtensions.GetMethodInfo(() => RefreshGameState()));
    }

    void UnregisterSOLuaFuncs()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(Calendar.CompleteCurrentEvent));
        Lua.UnregisterFunction(nameof(MiniGameManager.StartMiniGame));
        Lua.UnregisterFunction(nameof(MiniGameManager.StartGiftingMiniGame));
        Lua.UnregisterFunction(nameof(MiniGameManager.GetLastScreenPrintingScore));
        Lua.UnregisterFunction(nameof(MiniGameManager.GetLastBassMiniGameScore));
        Lua.UnregisterFunction(nameof(JamCoordinator.StartJam));
        Lua.UnregisterFunction(nameof(JamCoordinator.EndJam));
        Lua.UnregisterFunction(nameof(Characters.UnlockEmoji));
        Lua.UnregisterFunction(nameof(Characters.Emote));
        Lua.UnregisterFunction(nameof(Characters.Shoot));
        Lua.UnregisterFunction(nameof(Characters.Drink));
        Lua.UnregisterFunction(nameof(Characters.MoveYPos));
        Lua.UnregisterFunction(nameof(Characters.SetLayer));
        Lua.UnregisterFunction(nameof(Characters.EnableCharacter));
        Lua.UnregisterFunction(nameof(Characters.DisableCharacter));
        Lua.UnregisterFunction(nameof(Characters.DisableDialogueTrigger));
        Lua.UnregisterFunction(nameof(Characters.NPCWalkTo));
        Lua.UnregisterFunction(nameof(Characters.NPCSkateTo));
        Lua.UnregisterFunction(nameof(Characters.NPCKickFlip));
        Lua.UnregisterFunction(nameof(Characters.NPCStopSkating));
        Lua.UnregisterFunction(nameof(Characters.NPCSkateBetween));
        Lua.UnregisterFunction(nameof(Characters.NPCFaceLeft));
        Lua.UnregisterFunction(nameof(Characters.NPCFaceRight));
        Lua.UnregisterFunction(nameof(Characters.GetMostRecentGiftReaction));
        Lua.UnregisterFunction(nameof(Characters.RecordMostRecentGiftReaction));
        Lua.UnregisterFunction(nameof(InventoryManager.AddToMCInventory));
        Lua.UnregisterFunction(nameof(InventoryManager.AddItem));
        Lua.UnregisterFunction(nameof(InventoryManager.AddPerishableItem));
        Lua.UnregisterFunction(nameof(InventoryManager.RemoveItem));
        Lua.UnregisterFunction(nameof(InventoryManager.RemovePerishableItem));
        Lua.UnregisterFunction(nameof(InventoryManager.GetNumInPockets));
        Lua.UnregisterFunction(nameof(InventoryManager.FindPurchaseableTopOrOutfitName));
        Lua.UnregisterFunction(nameof(InventoryManager.BuyPurchaseableTopOrOutfit));
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
        Lua.UnregisterFunction(nameof(DailyRandoms.DrinkSpecial));
        Lua.UnregisterFunction(nameof(AudioController.PauseBGMusic));
        Lua.UnregisterFunction(nameof(AudioController.UnpauseBGMusic));
        Lua.UnregisterFunction(nameof(AudioController.Clap));
        Lua.UnregisterFunction(nameof(RefreshGameState));
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

    public void PauseBGMusic()
    {
        bgMusic.PauseAudio();
    }

    public void UnpauseBGMusic()
    {
        bgMusic.UnpauseAudio();
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
        JamCoordinator.Load();
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
        DailyRandoms.LoadFromJson().WaitForCompletion();
        ConversationJson.LoadFromJson().WaitForCompletion();
        ConversationJson.LoadQuestsFromJson().WaitForCompletion();
        ConversationJson.LoadRomancesFromJson().WaitForCompletion();
        BandJson.LoadFromJson().WaitForCompletion();
        JobSystem.Load();
        QuestManager.Load();
        RomanceManager.Load();
        Tutorial.Load();
        JamCoordinator.Load();
        if (Phone.Instance != null)
            Phone.Instance.Load();
    }

    private void SaveData()
    {
        JamCoordinator.Save();
        MainCharacterState.Save();
        Calendar.Save();
        InventoryManager.SaveInventories();
        JobSystem.Save();
        QuestManager.Save();
        RomanceManager.Save();
        Tutorial.Save();
        if (Phone.Instance != null)
            Phone.Instance.Save();
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
        //SaveData();

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
            buttonSounds = GetComponentInChildren<ButtonSoundEffects>(true);
            miscSoundEffects = GetComponentInChildren<MiscSoundEffects>(true);
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
