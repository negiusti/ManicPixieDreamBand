using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioController", menuName = "Custom/AudioController")]
public class AudioController : ScriptableObject
{
    public static void PauseBGMusic()
    {
        GameManager.Instance.PauseBGMusic();
    }

    public static void UnpauseBGMusic()
    {
        GameManager.Instance.UnpauseBGMusic();
    }
}
