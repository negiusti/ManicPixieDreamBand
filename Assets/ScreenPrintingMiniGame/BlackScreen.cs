using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    private ScreenPrintingMiniGame screenPrintingMiniGame;

    private TattooMiniGame tattooMiniGame;

    private void Start()
    {
        screenPrintingMiniGame = GetComponentInParent<ScreenPrintingMiniGame>();
        tattooMiniGame = GetComponentInParent<TattooMiniGame>();
    }

    public void CloseMiniGames()
    {
        if (screenPrintingMiniGame != null)
        {
            screenPrintingMiniGame.CloseMiniGame();
        }
        else if (tattooMiniGame != null)
        {
            tattooMiniGame.CloseMiniGame();
        }
    }
}
