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

    public static void NPCWalkTo(string npc, double targetX)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            characters[npc].GetComponent<NPCMovement>().WalkTo((float)targetX);
        }
    }

    public static void NPCSkateTo(string npc, double targetX)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            characters[npc].GetComponent<NPCMovement>().SkateTo((float)targetX);
        }
    }

    public static void NPCFaceRight(string npc)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            characters[npc].FaceRight();
        }
    }

    public static void NPCFaceLeft(string npc)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            characters[npc].FaceLeft();
        }
    }

    public static void EnableCharacter(string name)
    {
        if (characters == null || !characters.ContainsKey(name))
            RefreshCharactersCache();
        if (characters.ContainsKey(name))
            characters[name].gameObject.SetActive(true);
    }

    public static void DisableCharacter(string name)
    {
        if (characters == null || !characters.ContainsKey(name))
            RefreshCharactersCache();
        if (characters.ContainsKey(name))
            characters[name].gameObject.SetActive(false);
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
