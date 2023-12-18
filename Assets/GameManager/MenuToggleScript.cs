using UnityEngine;

public class MenuToggleScript : MonoBehaviour
{
    public GameObject menuToToggle;
    //private SpriteRenderer menuBackground;

    private void Start()
    {
        //menuBackground = this.GetComponentInChildren<SpriteRenderer>();
        DisableMenu();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuToToggle.activeSelf)
            {
                DisableMenu();
            }
            else
            {
                EnableMenu();
            }
        }
    }
    public void DisableMenu()
    {
        //menuBackground.enabled = false;
        menuToToggle.SetActive(false);
        Time.timeScale = 1f;
    }

    public void EnableMenu()
    {
        menuToToggle.SetActive(true);
        //menuBackground.enabled = true;
        Time.timeScale = 0f;
    }
}
