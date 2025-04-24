using PixelCrushers.DialogueSystem;
using UnityEngine;
using Rewired;

public class SplashScreen : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);

        DialogueManager.StopAllConversations();
        if (Phone.Instance != null)
        {
            Destroy(Phone.Instance.gameObject);
        }
        if (DialogueManager.Instance != null)
        {
            Destroy(DialogueManager.Instance.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Interact") && InteractionEnabled())
        {
            // FOR EXHIBITION DEMOS ONLY:
            //SaveSystem.DeleteSaveData();
            //GameManager.Instance.RefreshGameState();

            if (completedGame())
            {
                SaveSystem.DeleteSaveData();
                GameManager.Instance.RefreshGameState();
            }

            GameManager.Instance.OpenSaveSlots();
            //SceneChanger.Instance.ChangeScene("Bedroom");
            //SceneChanger.Instance.ChangeScene("LoadUserData");
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
        return currentConvoIdx == 18;
    }
}
