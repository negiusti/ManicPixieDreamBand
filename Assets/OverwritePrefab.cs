#if (UNITY_EDITOR) 
using UnityEngine;
using UnityEditor;
using System.IO;

public class OverwritePrefab : MonoBehaviour
{
    private GameObject instance; // Reference to your instance

    public void OverwritePrefabXD()
    {
        instance = GameObject.FindFirstObjectByType<Character>().gameObject;
        string prefabPath = "Assets/Prefabs/Characters/" + instance.name + ".prefab";
        Debug.Log(prefabPath);

        //if (File.Exists(prefabPath))
        //{
        //    Debug.Log("File already exists");
        //    return;
        //}
        //SaveSystem.SaveCharacter(instance.GetComponent<Character>());

        PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);

        Debug.Log("Prefab overwritten!");
    }
}
#endif