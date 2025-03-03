using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SplashArcadeScreen : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.StopAllConversations();
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && InteractionEnabled())
        {
            // FOR EXHIBITION DEMOS ONLY:
            //SaveSystem.DeleteSaveData();
            //GameManager.Instance.RefreshGameState();

            //if (completedGame())
            //{
            //    SaveSystem.DeleteSaveData();
            //    GameManager.Instance.RefreshGameState();
            //}

            SceneChanger.Instance.ChangeScene("Tattoo");
        }
    }

    private bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen() &&
            !MiniGameManager.AnyActiveMiniGames();
    }

    public void TrailerDone()
    {
        //sc.ChangeScene("Bedroom");
    }
}
