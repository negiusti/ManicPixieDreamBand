using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class SceneChanger : MonoBehaviour
{
    MenuToggleScript menuToggle;
    Stack<string> sceneStack;
    //private GameObject player;
    public void ChangeScene(string sceneName)
    {
        DialogueManager.StopAllConversations();
        //string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneName);
        //AsyncOperation loadOperation =
        SceneManager.LoadScene(sceneName);
        menuToggle.DisableMenu();
        sceneStack.Push(sceneName);
        //loadOperation.completed += (operation) => OnLoadCompleted(operation, currentScene);
    }

    public void GoToPreviousScene()
    {
        if (sceneStack.Peek() != null)
            sceneStack.Pop();
        if (sceneStack.Peek() != null)
            ChangeScene(sceneStack.Peek());
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(DebugLog), this, SymbolExtensions.GetMethodInfo(() => DebugLog(string.Empty)));
        Lua.RegisterFunction(nameof(ChangeScene), this, SymbolExtensions.GetMethodInfo(() => ChangeScene(string.Empty)));
    }

    void OnDisable()
    {
        //if (unregisterOnDisable)
        //{
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(nameof(DebugLog));
            Lua.UnregisterFunction(nameof(ChangeScene));
        //}
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    // Start is called before the first frame update
    void Start()
    {
        menuToggle = GetComponent<MenuToggleScript>();
        menuToggle.DisableMenu();
        sceneStack = new Stack<string>();
        sceneStack.Push("Bedroom");
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
