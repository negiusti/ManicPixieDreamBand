using System.Collections.Generic;
using System.Linq;
using PixelCrushers.DialogueSystem;
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
    //public static void Save()
    //{
    //    if (characters == null)
    //        RefreshCharactersCache();
    //    foreach (Character c in characters.Values)
    //    {
    //        c.SaveCharacter();
    //    }
    //}
    public static void RefreshCharactersCache()
    {
        characters = FindObjectsOfType<Character>(true)
            .Where(c => c.gameObject.layer != LayerMask.NameToLayer("LoadingScreen"))
            .ToDictionary(c => c.name, c => c);
        mc = characters.FirstOrDefault(c => c.Value.isMainCharacter()).Value;
    }

    public static Dictionary<string, Character> CharactersInScene() {
        if (mc == null)
            RefreshCharactersCache();
        return characters;
    }

    public static void DisableDialogueTrigger(string npc)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            if (characters[npc].gameObject.GetComponent<DialogueSystemTrigger>() != null)
                characters[npc].gameObject.GetComponent<DialogueSystemTrigger>().enabled = false;
            if (characters[npc].gameObject.GetComponent<Usable>() != null)
                characters[npc].gameObject.GetComponent<Usable>().enabled = false;
        }
    }

    public static void Emote(string character, string eyesEmotion, string mouthEmotion)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        bool changedMouth = characters[character].EmoteMouth(mouthEmotion);
        bool changedEyes = characters[character].EmoteEyes(eyesEmotion);
        if (changedEyes || changedMouth)
            characters[character].FacePop();
    }

    public static void MoveYPos(string character, double y)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (characters.ContainsKey(character))
        {
            characters[character].gameObject.transform.position = new Vector3(characters[character].gameObject.transform.position.x, (float)y, characters[character].gameObject.transform.position.z);
        }
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

    public static void SetConvoTrigger(string name, string convo)
    {
        if (characters == null || !characters.ContainsKey(name))
            RefreshCharactersCache();
        if (characters.ContainsKey(name))
        {
            if (characters[name].gameObject.GetComponent<Usable>() == null)
                characters[name].gameObject.AddComponent<Usable>();
            if (characters[name].gameObject.GetComponent<DialogueSystemTrigger>() == null)
                characters[name].gameObject.AddComponent<DialogueSystemTrigger>();

            DialogueSystemTrigger trigger = characters[name].gameObject.GetComponent<DialogueSystemTrigger>();
            trigger.trigger = DialogueSystemTriggerEvent.OnUse;
            trigger.conversation = convo;
        }
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
