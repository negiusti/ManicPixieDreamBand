using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    private Camera loadingCam;
    private Camera mainCam;

    private void Start()
    {
        // Hide the loading screen initially
        loadingCam = this.GetComponentInChildren<Camera>();
        //SceneManager.activeSceneChanged += OnSceneChange;
    }

    //public void OnSceneChange(Scene current, Scene next)
    //{
    //    Camera[] cams = Camera.allCameras;
    //    mainCam = cams.Where(c => !c.Equals(loadingCam)).First();
    //}

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchCams()
    {
        //mainCam = Camera.main;
        //if (loadingCam == null)
        //    loadingCam = this.GetComponentInChildren<Camera>();
        //loadingCam.enabled = true;
        //mainCam.enabled = false;
    }

    private void OnDisable()
    {
        //mainCam.enabled = true;
        //loadingCam.enabled = false;
    }

}
