using UnityEngine;

public class MenuToggleScript : MonoBehaviour
{
    public GameObject menuToToggle;

    private void Start()
    {
        menuToToggle.SetActive(false);
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Toggle the active state of the game object
            menuToToggle.SetActive(!menuToToggle.activeSelf);
            if (menuToToggle.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
