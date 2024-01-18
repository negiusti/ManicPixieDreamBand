using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

[CreateAssetMenu(fileName = "SpawnCharacters", menuName = "Custom/SpawnCharacters", order = 1)]
public class SpawnCharacters : ScriptableObject
{
    public void SpawnCharacter(string name, Vector3 position)
    {
        // character already exists in scene, don't spawn another plz
        if (FindObjectsOfType<Character>().Any(x => x.name == name))
            return;
        string characterPrefabPath = "Assets/Prefabs/Characters/"+ name + ".prefab";
        Addressables.LoadAssetAsync<GameObject>(characterPrefabPath).Completed += (operation) => OnPrefabLoaded(operation, position);
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle, Vector3 position)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate the prefab and spawn it in the current scene
            Instantiate(handle.Result, position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Failed to load prefab from Addressables: " + handle.OperationException);
        }
    }
}