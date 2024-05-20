using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    private ScreenPrintingMiniGame mg;

    private void Start()
    {
        mg = GetComponentInParent<ScreenPrintingMiniGame>();
    }

    public void CloseScreenPrinting()
    {
        mg.CloseMiniGame();
    }
}
