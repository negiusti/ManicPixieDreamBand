using System;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public abstract void OpenMiniGame();
    public abstract void CloseMiniGame();
    public abstract bool IsMiniGameActive();

    private void SetChildrenActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    protected void EnableAllChildren()
    {
        SetChildrenActive(true);
    }

    protected void DisableAllChildren()
    {
        SetChildrenActive(false);
    }
}
