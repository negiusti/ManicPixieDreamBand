using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public float minLoadingTime = 2f; // Minimum time to show loading screen (in seconds)

    private void Start()
    {
        // Hide the loading screen initially
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Wait for a minimum loading time to ensure loading screen is visible
        yield return new WaitForSeconds(minLoadingTime);

        // Begin loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous operation is complete
        while (!operation.isDone)
        {
            // Calculate the progress and update loading screen if needed
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 0.9f is the progress value when the scene is fully loaded
            Debug.Log("Loading progress: " + progress);

            // Optionally, update UI elements on the loading screen to show progress

            yield return null;
        }
    }
}
