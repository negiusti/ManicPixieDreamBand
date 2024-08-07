using System.Linq;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(fileName = "MiniGameManager", menuName = "Custom/MiniGameManager")]
public class MiniGameManager : ScriptableObject
{
    public static bool AnyActiveMiniGames() {
        //foreach (MiniGame mg in FindObjectsOfType<MiniGame>().Where(mg => mg.IsMiniGameActive())) {
        //    Debug.Log("ACTIVE MG: " + mg.name + " " + mg.gameObject.name);
        //}

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
            case "Boba":
                StartBobaMiniGame();
                break;
            case "BandName":
                StartBandNameMiniGame();
                break;
            case "DemoEnding":
                StartDemoEnding();
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
            case "Boba":
                return FindFirstObjectByType<BobaMiniGame>(FindObjectsInactive.Include);
            case "BandName":
                return FindFirstObjectByType<BandNameMiniGame>(FindObjectsInactive.Include);
            case "DemoEnding":
                return FindFirstObjectByType<DemoEndingMiniGame>(FindObjectsInactive.Include);
            default:
                Debug.LogError("Minigame not found: " + miniGameName);
                return null;
        }
    }

    public static void PrepMiniGame()
    {
        GameManager.Instance.GetComponent<MenuToggleScript>().DisableMenu();
        Debug.Log("disabling phone");
        //if (Phone.Instance != null)
        Phone.Instance.gameObject.SetActive(false);
        DialogueManager.Pause();
    }

    public static void CleanUpMiniGame()
    {
        Debug.Log("enabling phone");
        //if (Phone.Instance != null)
            Phone.Instance.gameObject.SetActive(true);
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

    private static void StartBobaMiniGame()
    {
        BobaMiniGame mg = (BobaMiniGame)GetMiniGame("Boba");
        mg.OpenMiniGame();
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

    private static void StartBandNameMiniGame()
    {
        BandNameMiniGame mg = (BandNameMiniGame)GetMiniGame("BandName");
        mg.OpenMiniGame();
    }

    private static void StartDemoEnding()
    {
        DemoEndingMiniGame mg = (DemoEndingMiniGame)GetMiniGame("DemoEnding");
        mg.OpenMiniGame();
    }

    public static bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            Phone.Instance.IsLocked() &&
            !DialogueManager.IsConversationActive &&
            !AnyActiveMiniGames();
    }
}
