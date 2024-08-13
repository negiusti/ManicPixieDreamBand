using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    //private GameManager gm;
    //private SceneChanger sc;
    //public VideoControl vc;


    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.StopAllConversations();
        //gm = GameManager.Instance;
        //sc = gm.GetComponent<SceneChanger>()
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && InteractionEnabled())
        {
            //vc.Play();
            //SceneChanger.Instance.ChangeScene("Bedroom");
            SaveSystem.DeleteSaveData();
            GameManager.Instance.RefreshGameState();
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
}
