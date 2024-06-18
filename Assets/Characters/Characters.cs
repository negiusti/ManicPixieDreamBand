using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Characters", menuName = "Custom/Characters")]
public class Characters : ScriptableObject
{
    private static Dictionary<string, Character> characters;
    private static Character mc;

    // keep a cache of currently loaded characters and refresh it on each scene change
    public static void RefreshCharactersCache(Scene scene, Scene mode)
    {
        RefreshCharactersCache();
    }

    public static void RefreshCharactersCache()
    {
        characters = FindObjectsOfType<Character>(true)
            .Where(c => c.gameObject.layer != LayerMask.NameToLayer("LoadingScreen"))
            .ToDictionary(c => c.name, c => c);
        mc = characters.FirstOrDefault(c => c.Value.isMainCharacter()).Value;
    }

    public static void Emote(string character, string eyesEmotion, string mouthEmotion)
    {
        if (characters == null)
            RefreshCharactersCache();
        characters[character]?.EmoteMouth(mouthEmotion);
        characters[character]?.EmoteEyes(eyesEmotion);
        characters[character]?.FacePop();
    }

    public static Character MainCharacter()
    {
        if (mc == null)
        {
            RefreshCharactersCache();
        }
        return mc;
    }
}
