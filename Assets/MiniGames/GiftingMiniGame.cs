
using UnityEngine;

public class GiftingMiniGame : MiniGame
{
    private bool isActive;
    private string npcName;

    private void Start()
    {
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        EnableAllChildren();
        MiniGameManager.PrepMiniGame();
        isActive = true;
    }

    public void OpenMiniGame(string npc)
    {
        npcName = npc;
        transform.position = Camera.main.transform.position;
        OpenMiniGame();
    }

    public void RegisterNPCReaction(string gift)
    {
        Debug.Log("RegisterNPCReaction: " + npcName + gift);
        switch (npcName)
        {
            case "JJ":
                switch (gift)
                {
                    case "Boba":
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Bad);
                        return;
                    case "Coffee":
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
                        return;
                    case "RootBeer":
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
                        return;
                    default:
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Mid);
                        return;
                }
            case "Pixie":
                switch (gift)
                {
                    case "Boba":
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
                        return;
                    case "Coffee":
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Mid);
                        return;
                    case "RootBeer":
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Bad);
                        return;
                    default:
                        Debug.Log("BITCH");
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Mid);
                        return;
                }
            case "Rex":
                switch (gift)
                {
                    case "Boba":
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Bad);
                        return;
                    case "Coffee":
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
                        return;
                    case "RootBeer":
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
                        return;
                    default:
                        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Mid);
                        return;
                }
            default:
                Debug.Log("BITCH");
                Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
                return;
        }
    }

    public void RegisterNVMReaction()
    {
        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Nvm);
        CloseMiniGame();
    }

    public override void CloseMiniGame()
    {
        DisableAllChildren();
        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    public void CloseMiniGame(string gift)
    {
        RegisterNPCReaction(gift);
        CloseMiniGame();
        Characters.Drink(npcName, gift);
    }
}
