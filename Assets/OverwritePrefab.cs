#if (UNITY_EDITOR) 
using UnityEngine;
using UnityEditor;
using System.IO;

public class OverwritePrefab : MonoBehaviour
{
    public GameObject instance; // Reference to your instance

    public void OverwritePrefabXD()
    {
        string prefabPath = "Assets/Prefabs/" + instance.name + ".prefab";
        Debug.Log(prefabPath);

        if (File.Exists(prefabPath))
        {
            Debug.Log("File already exists");
            return;
        }
            

        PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);

        Debug.Log("Prefab overwritten!");
    }
}
#endif