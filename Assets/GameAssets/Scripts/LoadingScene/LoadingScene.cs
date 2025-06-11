using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]private TMP_Text loadingText;
    // [SerializeField] private bool isSceneReady = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        StartCoroutine(LoadGameAsync());
    }

    private IEnumerator LoadGameAsync ()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Mainscene");
        asyncOperation.allowSceneActivation = false;

        float displayedProgress = 0f;

        while (!asyncOperation.isDone)
        {
            // Actual loading progress (0 to 0.9)
            float targetProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // Smoothly interpolate the displayed progress
            displayedProgress = Mathf.MoveTowards(displayedProgress , targetProgress , Time.deltaTime);

            loadingText.text = "Loading... " + Mathf.RoundToInt(displayedProgress * 100f) + "%";

            // If the loading is nearly done
            if (asyncOperation.progress >= 0.9f)
            {
                // Wait until your data is fetched
                if (GameManager.Instance.IsDataFetched())
                {
                    // Simulate smooth transition to 100%
                    while (displayedProgress < 1f)
                    {
                        displayedProgress = Mathf.MoveTowards(displayedProgress , 1f , Time.deltaTime);
                        loadingText.text = "Loading... " + Mathf.RoundToInt(displayedProgress * 100f) + "%";
                        yield return null;
                    }

                    //isSceneReady = true;
                    break;
                }
            }

            yield return null;
        }
        // Once everything is ready, activate the scene
        asyncOperation.allowSceneActivation = true;

        yield return null;

        // Unload the loading scene (assumes this script is in "LoadingScene")
        Scene loadingScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(loadingScene);
    }

}
