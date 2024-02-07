using PixelCrushers.DialogueSystem;
using UnityEngine;


public class MainCharacter : MonoBehaviour
{
    private ProximitySelector ps;
    public double moneyDEBUG = -1;

    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ProximitySelector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.isConversationActive && ps.enabled)
        {
            ps.enabled = false;
        } else if (!DialogueManager.isConversationActive && !ps.enabled)
        {
            ps.enabled = true;
        }
    }

    // EVERYTHING BELOW USED FOR LUA FUNCTIONS IN DIALOGUE TREE!!!!

    public static bool HasChangedOutfitToday()
    {
        return MainCharacterState.HasChangedOutfitToday();
    }

    public static double CurrentBankBalance()
    {
        Debug.Log(" CurrentBankBalance() is " + MainCharacterState.GetBankBalance());
        return MainCharacterState.GetBankBalance();
    }

    void OnEnable()
    {
        if (moneyDEBUG >= 0)
        {
            MainCharacterState.SetBankBalance(moneyDEBUG);
        }
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(HasChangedOutfitToday), this, SymbolExtensions.GetMethodInfo(() => HasChangedOutfitToday()));
        Lua.RegisterFunction(nameof(CurrentBankBalance), this, SymbolExtensions.GetMethodInfo(() => CurrentBankBalance()));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(HasChangedOutfitToday));
        Lua.UnregisterFunction(nameof(CurrentBankBalance));
        //}
    }
}
