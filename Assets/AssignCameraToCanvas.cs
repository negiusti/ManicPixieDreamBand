using UnityEngine.SceneManagement;
using UnityEngine;

public class AssignCameraToCanvas : MonoBehaviour
{
    private Canvas c;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        c = GetComponent<Canvas>();
        if (c == null)
        {
            c = GetComponentInChildren<Canvas>();
        }
        c.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main != c.worldCamera)
        {
            c.worldCamera = Camera.main;
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (c != null)
            c.worldCamera = Camera.main;
    }
}
