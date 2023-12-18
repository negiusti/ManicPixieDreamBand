using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private GameManager gm;
    private SceneChanger sc;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        sc = gm.GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sc.ChangeScene("Character_Editor");

        }
    }
}
