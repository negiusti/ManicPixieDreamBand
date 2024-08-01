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
        //gm = GameManager.Instance;
        //sc = gm.GetComponent<SceneChanger>()
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //vc.Play();
            //SceneChanger.Instance.ChangeScene("Bedroom");
            SaveSystem.DeleteSaveData();
            GameManager.Instance.RefreshGameState();
            SceneChanger.Instance.ChangeScene("Bedroom");
        }
    }

    public void TrailerDone()
    {
        //sc.ChangeScene("Bedroom");
    }
}
