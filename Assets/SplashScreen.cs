using PixelCrushers.DialogueSystem;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.StopAllConversations();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && InteractionEnabled())
        {
            // FOR EXHIBITION DEMOS ONLY:
            SaveSystem.DeleteSaveData();
            GameManager.Instance.RefreshGameState();

            //if (completedGame())
            //{
            //    SaveSystem.DeleteSaveData();
            //    GameManager.Instance.RefreshGameState();
            //}

            SceneChanger.Instance.ChangeScene("Bedroom");
        }
    }

    private bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            (Phone.Instance == null || Phone.Instance.IsLocked()) &&
            !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen() &&
            !DialogueManager.IsConversationActive &&
            !MiniGameManager.AnyActiveMiniGames();
    }

    public void TrailerDone()
    {
        //sc.ChangeScene("Bedroom");
    }

    private bool completedGame()
    {
        // TODO: update this for addtional plot convos
        int currentConvoIdx = ES3.Load("PlotConvoIdx", 0);
        Debug.Log("CUrrent convo IDX is: " + currentConvoIdx);
        return currentConvoIdx == 9;
    }
}
