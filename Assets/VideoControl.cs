using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    //public MenuToggleScript menu;
    //public GameObject screen;
    //private SpriteRenderer sr;
    //public SplashScreen splash;
    private GameManager gm;
    private SceneChanger sc;
    private bool skip;
    private float startTime;

    void Start()
    {
        // Get the VideoPlayer component if not assigned in the Inspector
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Attach a callback for when the video completes
        videoPlayer.loopPointReached += OnVideoEnd;
        //screen.SetActive(false);
        //sr = screen.GetComponent<SpriteRenderer>();
        //sr.enabled = false;
        gm = GameManager.Instance;
        sc = gm.GetComponent<SceneChanger>();
        skip = false;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    startTime = Time.time;
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    if (Time.time - startTime > 2f)
        //    {
        //        skip = true;
        //    }
        //}
        //if (skip)
        //{
        //    sc.ChangeScene("Character_Editor");
        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    startTime = Time.time;
        //    skip = false;
        //}
    }

    public void PlayPauseVideo()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        //sr.enabled = false;
        //if (splash != null)
        //    splash.TrailerDone();
        sc.ChangeScene("Character_Editor");
        Debug.Log("Video has ended!");
    }

    public void Play()
    {
        //menu.DisableMenu();
        //screen.SetActive(true);
        //sr.enabled = true;
        videoPlayer.Play();
    }

    public void GoToDemoPortrait()
    {
        sc.ChangeScene("DemoPortrait");
    }
}
