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
        RefreshCharactersCache();
    }

    public static void RefreshCharactersCache()
    {
        characters = FindObjectsOfType<Character>(true).ToDictionary(c => c.name, c => c);
    }

    public static void EmoteMouth(string character, string emotion)
    {
        if (characters == null)
            RefreshCharactersCache();
        characters[character]?.EmoteMouth(emotion);
    }

    public static void EmoteEyes(string character, string emotion)
    {
        if (characters == null)
            RefreshCharactersCache();
        characters[character]?.EmoteEyes(emotion);
    }
}
