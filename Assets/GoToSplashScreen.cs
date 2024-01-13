using UnityEngine;

public class GoToSplashScreen : MonoBehaviour
{
    private GameManager gm;
    private SceneChanger sc;
    private float startTime;
    private TakeMyPhoto takeMyPhoto;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        sc = gm.GetComponent<SceneChanger>();
        startTime = Time.time;
        takeMyPhoto = Camera.main.gameObject.GetComponent<TakeMyPhoto>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        takeMyPhoto.SaveSpriteAsPNG();
        GoToSplash();
    }

    public void GoToSplash()
    {
        // Prevent this button from being clicked too quickly
        // Can't believe unity is making me do this LOL
        if (Time.time - startTime < 2f)
        {
            return;
        }
        sc.ChangeScene("Splash");
    }

}
