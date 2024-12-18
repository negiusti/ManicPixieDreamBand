using System.Collections;
using System.Collections.Generic;
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
        OpenMiniGame();
    }

    public void RegisterNPCReaction(string gift)
    {
        // TO DO: record different reactions
        Characters.RecordMostRecentGiftReaction(Characters.CharacterGiftReaction.Good);
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
        CloseMiniGame();
        Characters.Drink(npcName, gift);
        RegisterNPCReaction(gift);
    }
}
