using System.Linq;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(fileName = "MiniGameManager", menuName = "Custom/MiniGameManager")]
public class MiniGameManager : ScriptableObject
{
    public static bool AnyActiveMiniGames() {
        foreach (MiniGame mg in FindObjectsOfType<MiniGame>().Where(mg => mg.IsMiniGameActive())) {
            Debug.Log("ACTIVE MG: " + mg.name + " " + mg.gameObject.name);
        }

        return FindObjectsOfType<MiniGame>().Any(mg => mg.IsMiniGameActive());
    }

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
            case "ScreenPrinting":
                StartScreenPrintingMiniGame();
                break;
            case "Tattoo":
                StartTattooMiniGame();
                break;
            case "Corkboard":
                StartCorkboardMiniGame();
                break;
            default:
                Debug.LogError("Minigame not found: " + miniGameName);
                break;
        }
    }

    public static MiniGame GetMiniGame(string miniGameName)
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
            case "ScreenPrinting":
                return FindFirstObjectByType<ScreenPrintingMiniGame>(FindObjectsInactive.Include);
            case "Tattoo":
                return FindFirstObjectByType<TattooMiniGame>(FindObjectsInactive.Include);
            case "Corkboard":
                return FindFirstObjectByType<CorkboardMiniGame>(FindObjectsInactive.Include);
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

    private static void StartScreenPrintingMiniGame()
    {
        ScreenPrintingMiniGame mg = (ScreenPrintingMiniGame)GetMiniGame("ScreenPrinting");
        mg.OpenMiniGame();
    }

    private static void StartTattooMiniGame()
    {
        TattooMiniGame mg = (TattooMiniGame)GetMiniGame("Tattoo");
        mg.OpenMiniGame();
    }

    private static void StartCorkboardMiniGame()
    {
        CorkboardMiniGame mg = (CorkboardMiniGame)GetMiniGame("Corkboard");
        mg.OpenMiniGame();
    }
}
