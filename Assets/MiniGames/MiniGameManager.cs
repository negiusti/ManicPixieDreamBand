using UnityEngine;
using System.Collections;
using System;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(fileName = "MiniGameManager", menuName = "Custom/MiniGameManager")]
public class MiniGameManager : ScriptableObject
{

    public static void StartMiniGame(string miniGameName)
    {
        switch (miniGameName)
        {
            case "BandPractice":
                StartBandPracticeMiniGame();
                break;
            case "Gig":
                StartGigMiniGame();
                break;
            case "Solo":
                StartBassMiniGame();
                break;
            default:
                Debug.LogError("Minigame not found: " + miniGameName);
                break;
        }
    }

    private MiniGame GetMiniGame(string miniGameName)
    {
        switch (miniGameName)
        {
            case "BandPractice":
                return FindFirstObjectByType<BassMiniGame>();
            case "Gig":
                return FindFirstObjectByType<BassMiniGame>();
            case "Solo":
                return FindFirstObjectByType<BassMiniGame>();
            default:
                Debug.LogError("Minigame not found: " + miniGameName);
                return null;
        }
    }

    public static void PrepMiniGame()
    {
        DialogueManager.Pause();
    }

    public static void CleanUpMiniGame()
    {
        DialogueManager.Unpause();
        DialogueManager.standardDialogueUI.OnContinueConversation();
    }

    private static void StartBassMiniGame()
    {
        BassMiniGame mg = FindFirstObjectByType<BassMiniGame>();
        mg.OpenMiniGame();
    }

    private static void StartGigMiniGame()
    {
        BassMiniGame mg = FindFirstObjectByType<BassMiniGame>();
        mg.StartBassMiniGameWithBand(false);
    }

    private static void StartBandPracticeMiniGame()
    {
        BassMiniGame mg = FindFirstObjectByType<BassMiniGame>();
        mg.StartBassMiniGameWithBand(true);
    }
}
