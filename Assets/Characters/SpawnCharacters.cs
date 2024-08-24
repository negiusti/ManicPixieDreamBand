using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnCharacters", menuName = "Custom/SpawnCharacters")]
public class SpawnCharacters : ScriptableObject
{
    private static AsyncOperationHandle<GameObject> SpawnParticipant(Participant p, string convo = null)
    {
        // character already exists in scene, don't spawn another plz
        if (FindObjectsOfType<Character>().Any(x => x.name == p.name))
        {
            Debug.Log("Not spawning: " + p.name + " because they're already in the scene");
            return new AsyncOperationHandle<GameObject>();
        }
        string characterPrefabPath = "Assets/Prefabs/Characters/" + p.name + ".prefab";
        AsyncOperationHandle<GameObject> operation = Addressables.LoadAssetAsync<GameObject>(characterPrefabPath);
        operation.Completed += (operation) => OnPrefabLoaded(operation, p, convo);
        return operation;
    }

    public static void SpawnParticipants(Participant[] participants, string convo = null)
    {
        Debug.Log("SpawnParticipants");
        if (participants == null)
            return;
        Dictionary<string, Character> characters = Characters.CharactersInScene();

        // Sort characters by their y position
        Array.Sort(participants, (a, b) => b.position.y.CompareTo(a.position.y));
        Dictionary<string, int> layerToIdx = new Dictionary<string, int>();
        foreach (Participant p in participants)
        {
            Debug.Log("SpawnParticipant: " + p.name);
            Character c = characters.GetValueOrDefault(p.name, null);
            int idx = layerToIdx.GetValueOrDefault(p.layer, -1) + 1;
            layerToIdx[p.layer] = idx;
            if (c == null)
            {
                c = SpawnParticipant(p, convo).WaitForCompletion().GetComponent<Character>();
                Debug.Log("Spawned Participant: " + c.name + " " + c.gameObject.name);
                if (c.gameObject.name.Contains("Clone"))
                {
                    Debug.Log("Removing clone from name: " + c.gameObject.name);
                    // Remove "(Clone)" from the end of the name
                    Debug.Log("Setting name from/to: " + c.gameObject.name + " " + p.name);
                    c.gameObject.name = p.name;
                    c.SetCharacterName(p.name);
                    Debug.Log("Name is: " + c.gameObject.name);
                }
                c.gameObject.name = p.name;
            }
            else
            {
                Vector3 newPos = new Vector3(p.position.x, p.position.y, 3f);
                c.transform.position = newPos;
            }
            c.MoveToRenderLayer(p.layer, idx);
            //Debug.Log("Set to active: " + c.name + " " + p.existAtStart + " " + c.gameObject.name);
            //c.gameObject.SetActive(p.existAtStart);
        }
    }

    private static AsyncOperationHandle<GameObject> SpawnBandMember(BandMember m, Stage s)
    {
        string characterPrefabPath = "Assets/Prefabs/Characters/" + m.name + ".prefab";
        AsyncOperationHandle<GameObject> operation = Addressables.LoadAssetAsync<GameObject>(characterPrefabPath);
        operation.Completed += (operation) => OnPrefabLoaded(operation, m, s);
        return operation;
    }

    public static void SpawnBandMembers(BandMember[] members)
    {
        Stage stage = FindFirstObjectByType<Stage>();
        Character[] characters = FindObjectsOfType<Character>();
        foreach (BandMember m in members)
        {
            // character already exists in scene, don't spawn another plz
            Character c = characters.FirstOrDefault(c => c.name.Equals(m.name));
            if (c == null)
            {
                c = SpawnBandMember(m, stage).WaitForCompletion().GetComponent<Character>();
                c.MoveToRenderLayer("stage_char", 0);
            }
        }
    }

    private static void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle, Participant p, string convo = null)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate the prefab and spawn it in the current scene
            GameObject spawnedCharacter = Instantiate(handle.Result, p.position, Quaternion.identity);
            spawnedCharacter.name = handle.Result.name;

            if (p.faceLeftOrRight != null)
            {
                switch (p.faceLeftOrRight)
                {
                    case "left":
                        Characters.NPCFaceLeft(p.name);
                        break;
                    case "right":
                        Characters.NPCFaceRight(p.name);
                        break;
                    default:
                        break;
                }
            }

            if (p.isTrigger)
            {
                if (convo == null)
                {
                    Debug.LogError("why tf is the convo null here");
                    return;
                }
                if (spawnedCharacter.gameObject.GetComponent<Usable>() == null)
                    spawnedCharacter.gameObject.AddComponent<Usable>();
                if (spawnedCharacter.gameObject.GetComponent<DialogueSystemTrigger>() == null)
                    spawnedCharacter.gameObject.AddComponent<DialogueSystemTrigger>();

                DialogueSystemTrigger trigger = spawnedCharacter.gameObject.GetComponent<DialogueSystemTrigger>();
                trigger.trigger = DialogueSystemTriggerEvent.OnUse;
                trigger.conversation = convo;
                trigger.enabled = true;
            }
            else
            {
                if (spawnedCharacter.gameObject.GetComponent<DialogueSystemTrigger>() != null)
                    spawnedCharacter.gameObject.GetComponent<DialogueSystemTrigger>().enabled = false;
                if (spawnedCharacter.gameObject.GetComponent<Usable>() != null)
                    spawnedCharacter.gameObject.GetComponent<Usable>().enabled = false;
            }
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + handle.OperationException);
        }
    }

    private static void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle, BandMember m, Stage s)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {

            // Instantiate the prefab and spawn it in the current scene
            GameObject spawnedCharacter = Instantiate(handle.Result, s.GetInstrument(m.position).SpawnPos(), Quaternion.identity);
            //string originalName = spawnedCharacter.name;

            // Check if the name ends with "(Clone)"
            //if (originalName.EndsWith("(Clone)"))
            //{
            //    // Remove "(Clone)" from the end of the name
            //    string newName = originalName.Substring(0, originalName.Length - "(Clone)".Length);

            //    // Set the new name to the game object
            //    spawnedCharacter.name = newName;
            //    spawnedCharacter.GetComponent<Character>().SetCharacterName(newName);
            //}
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + handle.OperationException);
        }
    }

}