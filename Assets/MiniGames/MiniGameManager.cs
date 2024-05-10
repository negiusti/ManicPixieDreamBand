using UnityEngine;
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
            case "CarPacking":
                StartCarPackingMiniGame();
                break;
            default:
                Debug.LogError("Minigame not found: " + miniGameName);
                break;
        }
    }

    private static MiniGame GetMiniGame(string miniGameName)
    {
        switch (miniGameName)
        {
            case "BandPractice":
                return FindFirstObjectByType<BassMiniGame>(FindObjectsInactive.Include);
            case "Gig":
                return FindFirstObjectByType<BassMiniGame>(FindObjectsInactive.Include);
            case "Solo":
                return FindFirstObjectByType<BassMiniGame>(FindObjectsInactive.Include);
            case "CarPacking":
                return FindFirstObjectByType<CarPackingMiniGame>(FindObjectsInactive.Include);
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
        BassMiniGame mg = (BassMiniGame)GetMiniGame("Solo");
        mg.OpenMiniGame();
    }

    private static void StartGigMiniGame()
    {
        BassMiniGame mg = (BassMiniGame)GetMiniGame("Gig");
        mg.StartBassMiniGameWithBand(false);
    }

    private static void StartBandPracticeMiniGame()
    {
        BassMiniGame mg = (BassMiniGame)GetMiniGame("BandPractice");
        mg.StartBassMiniGameWithBand(true);
    }

    private static void StartCarPackingMiniGame()
    {
        CarPackingMiniGame mg = (CarPackingMiniGame)GetMiniGame("CarPacking");
        mg.OpenMiniGame();
    }
}
