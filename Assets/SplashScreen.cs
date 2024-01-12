using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private GameManager gm;
    private SceneChanger sc;
    //public VideoControl vc;
    

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        sc = gm.GetComponent<SceneChanger>();
        SaveSystem.DeleteMainCharacterData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //vc.Play();
            sc.ChangeScene("Trailer");
        }
    }

    public void TrailerDone()
    {
        sc.ChangeScene("Character_Editor");
    }
}
