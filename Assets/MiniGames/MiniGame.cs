using System;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    public abstract void OpenMiniGame();
    public abstract void CloseMiniGame();
    public abstract bool IsMiniGameActive();
}
