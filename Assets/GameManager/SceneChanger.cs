using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class SceneChanger : MonoBehaviour
{
    private static float minLoadingTime = 2f; // Minimum time to show loading screen (in seconds)
    public static SceneChanger Instance;
    private LoadingScreen loadingScreen;
    MenuToggleScript menuToggle;
    Stack<string> sceneStack;

    public bool IsLoadingScreenOpen()
    {
        return loadingScreen != null && loadingScreen.isActiveAndEnabled;
    }

    public void ChangeScene(string sceneName)
    {
        SaveCharacters();
        DialogueManager.StopAllConversations();
        //string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneName);
        //AsyncOperation loadOperation =


        LoadScene(sceneName);

        menuToggle.DisableMenu();
        sceneStack.Push(sceneName);
        //loadOperation.completed += (operation) => OnLoadCompleted(operation, currentScene);
    }

    public void LoadScene(string sceneName)
    {
        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.gameObject.SetActive(true);
            //loadingScreen.SwitchCams();
        }

        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Wait for a minimum loading time to ensure loading screen is visible
        yield return new WaitForSeconds(minLoadingTime);

        // Wait until the asynchronous operation is complete
        while (!operation.isDone)
        {
            // Calculate the progress and update loading screen if needed
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 0.9f is the progress value when the scene is fully loaded
            Debug.Log("Loading progress: " + progress);

            // Optionally, update UI elements on the loading screen to show progress

            yield return null;
        }
        if (loadingScreen != null)
        {
            loadingScreen.gameObject.SetActive(false);
        }
    }

    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void GoToPreviousScene()
    {
        if (sceneStack.Peek() != null)
            sceneStack.Pop();
        if (sceneStack.Peek() != null)
            ChangeScene(sceneStack.Peek());
    }

    private void SaveCharacters()
    {
        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character c in characters)
        {
            c.SaveCharacter();
        }
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
        loadingScreen = FindFirstObjectByType<LoadingScreen>(FindObjectsInactive.Include);
        menuToggle = GetComponent<MenuToggleScript>();
        menuToggle.DisableMenu();
        sceneStack = new Stack<string>();
        sceneStack.Push("Bedroom");
        
        if (Instance == null)
        {
            Instance = this;
        }
        //player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //DontDestroyOnLoad(gameObject);
    }
}
