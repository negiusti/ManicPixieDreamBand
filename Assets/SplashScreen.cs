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
        //sc = gm.GetComponent<SceneChanger>();

        //TODO for demo
        //SaveSystem.DeleteSaveData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //vc.Play();
            //SceneChanger.Instance.ChangeScene("Bedroom");
            SceneChanger.Instance.ChangeScene("Bedroom");
        }
    }

    public void TrailerDone()
    {
        //sc.ChangeScene("Bedroom");
    }
}
