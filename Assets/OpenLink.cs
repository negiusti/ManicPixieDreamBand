using UnityEngine;

public class OpenLink : MonoBehaviour
{
    [SerializeField]
    private string url = "https://store.steampowered.com/app/2772090/Punk_Juice/";

    private void OnMouseUpAsButton()
    {
        Application.OpenURL(url);
    }
}
