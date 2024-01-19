using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using PixelCrushers.DialogueSystem;

[CreateAssetMenu(fileName = "SpawnCharacters", menuName = "Custom/SpawnCharacters", order = 1)]
public class SpawnCharacters : ScriptableObject
{
    public static AsyncOperationHandle<GameObject> SpawnCharacter(Participant p)
    {
        // character already exists in scene, don't spawn another plz
        if (FindObjectsOfType<Character>().Any(x => x.name == p.name))
        {
            Debug.Log("Not spawning: " + p.name + " because they're already in the scene");
            return new AsyncOperationHandle<GameObject>();
        }
        string characterPrefabPath = "Assets/Prefabs/Characters/"+ p.name + ".prefab";
        AsyncOperationHandle<GameObject> operation = Addressables.LoadAssetAsync<GameObject>(characterPrefabPath);
        operation.Completed += (operation) => OnPrefabLoaded(operation, p);
        return operation;
    }

    private static void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle, Participant p)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate the prefab and spawn it in the current scene
            GameObject spawnedCharacter = Instantiate(handle.Result, p.position, Quaternion.identity);
            spawnedCharacter.GetComponent<Usable>().enabled = p.existAtStart;
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + handle.OperationException);
        }
    }
}