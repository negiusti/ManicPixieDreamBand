using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class SceneChanger : MonoBehaviour
{
    private static float minLoadingTime = 2f; // Minimum time to show loading screen (in seconds)
    public static SceneChanger Instance;
    public LoadingScreen genericLoadingScreen;
    public LoadingScreen busLoadingScreen;
    MenuToggleScript menuToggle;
    Stack<SceneInfo> sceneStack;

    public class SceneInfo
    {
        public string sceneName;
        public Vector3 position;
        public SceneInfo(string sceneName, Vector3 position)
        {
            this.sceneName = sceneName;
            this.position = position;
        }
    }

    public enum LoadingScreenType
    {
        Bus,
        Generic
    }

    public bool IsLoadingScreenOpen()
    {
        return (genericLoadingScreen != null && genericLoadingScreen.isActiveAndEnabled) ||
            (busLoadingScreen != null && busLoadingScreen.isActiveAndEnabled);
    }

    public void ChangeScene(string sceneName)
    {
        ChangeScene(sceneName, LoadingScreenType.Generic);
    }

    public void ChangeScene(SceneInfo sceneInfo)
    {
        ChangeScene(sceneInfo, LoadingScreenType.Generic);
    }

    public void ChangeScene(string sceneName, LoadingScreenType loadingScreenType)
    {
        PushCurrSceneToSceneStack();
        SaveCharacters();
        DialogueManager.StopAllConversations();
        //string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneName);
        //AsyncOperation loadOperation =

        LoadScene(sceneName, loadingScreenType);

        menuToggle.DisableMenu();
        //sceneStack.Push(new SceneInfo(sceneName, Characters.MainCharacter().transform.position));
        //loadOperation.completed += (operation) => OnLoadCompleted(operation, currentScene);
    }

    private void PushCurrSceneToSceneStack()
    {
        sceneStack.Push(new SceneInfo(GetCurrentScene(), Characters.MainCharacter().transform.position));
        Debug.Log("PUSH SCENE: " + GetCurrentScene());
    }

    private void ChangeScene(SceneInfo sceneInfo, LoadingScreenType loadingScreenType)
    {
        PushCurrSceneToSceneStack();
        SaveCharacters();
        DialogueManager.StopAllConversations();
        //string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneInfo.sceneName);
        //AsyncOperation loadOperation =
        LoadScene(sceneInfo, loadingScreenType);

        menuToggle.DisableMenu();
        //sceneStack.Push(new SceneInfo(sceneInfo.sceneName, Characters.MainCharacter().transform.position));
        //loadOperation.completed += (operation) => OnLoadCompleted(operation, currentScene);
    }

    public void LoadScene(string sceneName, LoadingScreenType loadingScreenType)
    {
        // Show loading screen
        if (genericLoadingScreen != null && loadingScreenType == LoadingScreenType.Generic)
        {
            genericLoadingScreen.gameObject.SetActive(true);
        }
        if (busLoadingScreen != null && loadingScreenType == LoadingScreenType.Bus)
        {
            busLoadingScreen.gameObject.SetActive(true);
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
        if (genericLoadingScreen != null)
        {
            genericLoadingScreen.gameObject.SetActive(false);
        }
        if (busLoadingScreen != null)
        {
            busLoadingScreen.gameObject.SetActive(false);
        }
    }

    private void LoadScene(SceneInfo sceneInfo, LoadingScreenType loadingScreenType)
    {
        // Show loading screen
        if (genericLoadingScreen != null && loadingScreenType == LoadingScreenType.Generic)
        {
            genericLoadingScreen.gameObject.SetActive(true);
        }
        if (busLoadingScreen != null && loadingScreenType == LoadingScreenType.Bus)
        {
            busLoadingScreen.gameObject.SetActive(true);
        }

        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync(sceneInfo));
    }

    private IEnumerator LoadSceneAsync(SceneInfo sceneInfo)
    {
        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneInfo.sceneName);

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
        Characters.MainCharacter().transform.position = sceneInfo.position;
        if (genericLoadingScreen != null)
        {
            genericLoadingScreen.gameObject.SetActive(false);
        }
        if (busLoadingScreen != null)
        {
            busLoadingScreen.gameObject.SetActive(false);
        }
    }

    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void GoToPreviousScene()
    {
        if (sceneStack.Peek() != null)
        {
            SaveCharacters();
            DialogueManager.StopAllConversations();
            //string currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("Popping scene: " + sceneStack.Peek().sceneName);
            //AsyncOperation loadOperation =
            //LoadScene(sceneStack.Pop().sceneName, LoadingScreenType.Generic);
            LoadScene(sceneStack.Pop(), LoadingScreenType.Generic);

            menuToggle.DisableMenu();
        }
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
        if (Instance == null)
        {
            Instance = this;
        }
        menuToggle = GetComponent<MenuToggleScript>();
        menuToggle.DisableMenu();
        sceneStack = new Stack<SceneInfo>();
        sceneStack.Push(new SceneInfo("DowntownNeighborhood", new Vector3(-57f, -2.4f, 0f)));
        
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
