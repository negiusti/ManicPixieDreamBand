using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Characters", menuName = "Custom/Characters")]
public class Characters : ScriptableObject
{
    private static Dictionary<string, Character> characters;

    // keep a cache of currently loaded characters and refresh it on each scene change
    public static void RefreshCharactersCache(Scene current, Scene next)
    {
        characters = FindObjectsOfType<Character>(true).ToDictionary(c => c.name, c => c);
    }
}
