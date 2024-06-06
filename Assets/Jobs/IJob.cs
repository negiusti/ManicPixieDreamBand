using UnityEngine;
public interface IJob
{
    string Location();
    string Name();
    GameObject Minigame();
    bool IsNight();
}
