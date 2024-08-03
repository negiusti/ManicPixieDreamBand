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
        //SaveCharacters();
        if (DialogueManager.Instance != null)
            DialogueManager.StopAllConversations();
        //string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneName);
        //AsyncOperation loadOperation =

        LoadScene(sceneName, loadingScreenType);
        if (menuToggle != null)
            menuToggle.DisableMenu();
        //sceneStack.Push(new SceneInfo(sceneName, Characters.MainCharacter().transform.position));
        //loadOperation.completed += (operation) => OnLoadCompleted(operation, currentScene);
    }

    private void PushCurrSceneToSceneStack()
    {
        sceneStack.Push(new SceneInfo(GetCurrentScene(), Characters.MainCharacter() == null ? Vector3.zero : Characters.MainCharacter().transform.position));
        Debug.Log("PUSH SCENE: " + GetCurrentScene());
    }

    private void ChangeScene(SceneInfo sceneInfo, LoadingScreenType loadingScreenType)
    {
        PushCurrSceneToSceneStack();
        //SaveCharacters();
        DialogueManager.StopAllConversations();
        //string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Loading scene: " + sceneInfo.sceneName);
        //AsyncOperation loadOperation =
        LoadScene(sceneInfo, loadingScreenType);
        if (menuToggle != null)
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

    void DisableLoadingScreens()
    {
        if (genericLoadingScreen != null)
        {
            genericLoadingScreen.gameObject.SetActive(false);
        }
        if (busLoadingScreen != null)
        {
            busLoadingScreen.gameObject.SetActive(false);
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        // Minimum time for loading screen
        yield return new WaitForSeconds(minLoadingTime);
        // Wait until the asynchronous scene fully loads
        while (!operation.isDone)
        {
            yield return new WaitForSeconds(1);
        }
        DisableLoadingScreens();
        yield return null;
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

        // Minimum time for loading screen
        yield return new WaitForSeconds(minLoadingTime);
        // Wait until the asynchronous scene fully loads
        while (!operation.isDone)
        {
            yield return new WaitForSeconds(1);
        }
        DisableLoadingScreens();
        ResetMCPosition(sceneInfo);
        yield return null;
    }

    private void ResetMCPosition(SceneInfo sceneInfo) {
        if (Characters.MainCharacter() == null)
            return;
        Characters.MainCharacter().transform.position = sceneInfo.position;
    }

    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void GoToPreviousScene()
    {
        if (sceneStack.Peek() != null)
        {
            //SaveCharacters();
            DialogueManager.StopAllConversations();
            //string currentScene = SceneManager.GetActiveScene().name;
            Debug.Log("Popping scene: " + sceneStack.Peek().sceneName);
            //AsyncOperation loadOperation =
            //LoadScene(sceneStack.Pop().sceneName, LoadingScreenType.Generic);
            LoadScene(sceneStack.Pop(), LoadingScreenType.Generic);
            if (menuToggle != null)
                menuToggle.DisableMenu();
        }
    }

    //private void SaveCharacters()
    //{
    //    Characters.Save();
    //}

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
        if (menuToggle != null)
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
