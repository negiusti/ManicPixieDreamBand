using UnityEngine;
public interface Job
{
    string Location();
    string Name();
    GameObject Minigame();
    bool IsNight();
}
