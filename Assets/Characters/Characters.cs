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
    private static CharacterGiftReaction mostRecentGiftReaction;

    public enum CharacterGiftReaction
    {
        Good,
        Mid,
        Bad,
        Nvm
    };

    public static void RecordMostRecentGiftReaction(string reaction)
    {
        switch (reaction.ToLower())
        {
            case "good":
                mostRecentGiftReaction = CharacterGiftReaction.Good;
                break;
            case "mid":
                mostRecentGiftReaction = CharacterGiftReaction.Mid;
                break;
            case "bad":
                mostRecentGiftReaction = CharacterGiftReaction.Bad;
                break;
            case "nvm":
                mostRecentGiftReaction = CharacterGiftReaction.Nvm;
                break;
            default:
                Debug.LogError("Invalid gift reaction: " + reaction);
                mostRecentGiftReaction = CharacterGiftReaction.Good;
                break;
        }
    }

    public static void RecordMostRecentGiftReaction(CharacterGiftReaction reaction)
    {
        mostRecentGiftReaction = reaction;
    }

    public static string GetMostRecentGiftReaction()
    {
        switch (mostRecentGiftReaction)
        {
            case CharacterGiftReaction.Good:
                return "good";
            case CharacterGiftReaction.Mid:
                return "mid";
            case CharacterGiftReaction.Bad:
                return "bad";
            case CharacterGiftReaction.Nvm:
                return "nvm";
            default: // should never happen
                Debug.LogError("Gift reaction not found: " + mostRecentGiftReaction.ToString());
                return "nvm";
        }
    }

    public static void UnlockEmoji(string contactName, string emojiName)
    {
        Phone.Instance.UnlockEmoji(contactName, emojiName);
    }

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
            .Where(c => c.gameObject.layer != LayerMask.NameToLayer("MiniGame"))
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

    public static void Teleport(string character, double x, double y, string layer, double idx)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        characters[character].Teleport((float)x, (float)y, layer, (int)idx);
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

    public static void Drink(string character, string itemName)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        characters[character].GetComponent<Movement>().Drink(itemName);
    }

    public static void Shoot(string character)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        characters[character].GetComponent<Movement>().Shoot();
    }

    public static void Kiss(string character)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        characters[character].GetComponent<Movement>().Kiss();
    }

    public static void SetTop(string character, string label)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        characters[character].SetTop(label);
    }

    public static void SetFaceDetail(string character, string label)
    {
        if (characters == null || !characters.ContainsKey(character))
            RefreshCharactersCache();
        if (!characters.ContainsKey(character))
        {
            Debug.LogError("Couldn't find character: " + character);
            return;
        }
        characters[character].SetFaceDetail(label);
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

    public static void SetLayer(string npc, string layer, double layerNum)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            characters[npc].MoveToRenderLayer(layer, (int)layerNum);
        }
    }

    //public static void SortCharacterLayers()
    //{
    //    Character[] characters = CharactersInScene().Values.ToArray();

    //    // Sort characters by their y position
    //    System.Array.Sort(characters, (a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
    //    Dictionary<string, int> layerToIdx = new Dictionary<string, int>();
    //    foreach (Character c in characters)
    //    {
    //        Debug.Log("SpawnParticipant: " + c.name);
    //        SortingGroup sortingGroup = c.GetComponent<SortingGroup>();
    //        int idx = layerToIdx.GetValueOrDefault(sortingGroup.sortingLayerName, -1) + 1;
    //        layerToIdx[sortingGroup.sortingLayerName] = idx;
    //        c.MoveToRenderLayer(sortingGroup.sortingLayerName, idx);
    //    }
    //}

    public static void NPCWalkTo(string npc, double targetX)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            if (npc == "MainCharacter" && characters[npc].GetComponent<NPCMovement>() == null)
            {
                MainCharacter().gameObject.AddComponent<NPCMovement>();
            }
            
            characters[npc].GetComponent<NPCMovement>().WalkTo((float)targetX);
        }
    }

    public static void NPCSkateBetween(string npc, double minx, double maxx, double seconds)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            characters[npc].GetComponent<NPCMovement>().SkateBetween((float)minx, (float)maxx, (float)seconds);
        }
    }

    public static void NPCStopSkating(string npc)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            if (npc == "MainCharacter" && characters[npc].GetComponent<NPCMovement>() == null)
            {
                MainCharacter().gameObject.AddComponent<NPCMovement>();
            }
            characters[npc].GetComponent<NPCMovement>().StopSkating();
        }
    }

    public static void NPCSkateTo(string npc, double targetX)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            if (npc == "MainCharacter" && characters[npc].GetComponent<NPCMovement>() == null)
            {
                MainCharacter().gameObject.AddComponent<NPCMovement>();
            }
            characters[npc].GetComponent<NPCMovement>().SkateTo((float)targetX);
        }
    }

    public static void NPCKickFlip(string npc)
    {
        if (characters == null || !characters.ContainsKey(npc))
            RefreshCharactersCache();
        if (characters.ContainsKey(npc))
        {
            characters[npc].gameObject.SetActive(true);
            if (npc == "MainCharacter" && characters[npc].GetComponent<NPCMovement>() == null)
            {
                MainCharacter().gameObject.AddComponent<NPCMovement>();
            }
            characters[npc].GetComponent<NPCMovement>().Flip();
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
        {
            characters[name].gameObject.transform.localScale = new Vector3(0.91f, 0.91f, 0.91f);
            characters[name].gameObject.SetActive(true);
        }
    }

    public static void DisableCharacter(string name)
    {
        if (characters == null || !characters.ContainsKey(name))
            RefreshCharactersCache();
        if (characters.ContainsKey(name))
            characters[name].gameObject.transform.localScale = Vector3.zero;
        //characters[name].gameObject.SetActive(false);
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