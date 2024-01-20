using PixelCrushers.DialogueSystem;
using UnityEngine;


public class MainCharacter : MonoBehaviour
{
    private ProximitySelector ps;

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

    void OnEnable()
    {
        Debug.Log("registering func");
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(HasChangedOutfitToday), this, SymbolExtensions.GetMethodInfo(() => HasChangedOutfitToday()));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
        // Remove the functions from Lua: (Replace these lines with your own.)
        Lua.UnregisterFunction(nameof(HasChangedOutfitToday));
        //}
    }
}
