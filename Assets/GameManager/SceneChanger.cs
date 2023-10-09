using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class SceneChanger : MonoBehaviour
{
    //private GameObject player;
    public void ChangeScene(string sceneName)
    {
        DialogueManager.StopAllConversations();
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneName);
        //AsyncOperation loadOperation =
        SceneManager.LoadSceneAsync(sceneName);
        //loadOperation.completed += (operation) => OnLoadCompleted(operation, currentScene);
    }

    //private void OnLoadCompleted(AsyncOperation operation, string scene)
    //{
    //    Debug.Log("Unloading scene: " + scene);
    //    SceneManager.UnloadSceneAsync(scene);
    //}

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
}
