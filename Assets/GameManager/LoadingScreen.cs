using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadingScreen : MonoBehaviour
{
    //public static LoadingScreen Instance;
    private Camera loadingCam;
    private Camera mainCam;

    private void Start()
    {
        // Hide the loading screen initially
        //DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
        //SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnEnable()
    {
        //loadingCam = this.GetComponentInChildren<Camera>();
        //mainCam = Camera.main;
        //loadingCam.enabled = true;
        //mainCam.enabled = false;
    }

    //public void OnSceneChange(Scene current, Scene next)
    //{
    //    Camera[] cams = Camera.allCameras;
    //    mainCam = cams.Where(c => !c.Equals(loadingCam)).First();
    //}

    public void SwitchCams()
    {
        loadingCam = this.GetComponentInChildren<Camera>();
        //mainCam = Camera.main;
        loadingCam.enabled = true;
        //mainCam.enabled = false;
    }

    private void OnDisable()
    {
        //mainCam.enabled = true;
        //loadingCam.enabled = false;
    }

}
