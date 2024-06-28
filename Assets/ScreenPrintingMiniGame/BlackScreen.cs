using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    private MiniGame mg;

    private void Start()
    {
        mg = GetComponentInParent<MiniGame>();
    }

    public void CloseMiniGames()
    {
        if (mg != null)
        {
            mg.CloseMiniGame();
        }
    }
}
