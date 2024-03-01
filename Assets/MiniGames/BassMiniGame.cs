using UnityEngine;

public class BassMiniGame : MiniGame
{
    private bool isActive;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CloseMiniGame();
        }
    }

    public override void OpenMiniGame()
    {
        isActive = true;
        EnableAllChildren();
    }

    public override void CloseMiniGame()
    {
        isActive = false;
        JamCoordinator.EndJam();
        DisableAllChildren();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    private void SetChildrenActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    private void EnableAllChildren()
    {
        SetChildrenActive(true);
    }

    private void DisableAllChildren()
    {
        SetChildrenActive(false);
    }
}
