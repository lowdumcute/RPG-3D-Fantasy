using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance;
    public GameObject loadingScreen;
    [SerializeField] private Slider ProgressBar;
    public Animator loadingAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float loadStartTime = Time.time;
        bool isSceneReady = false;

        while (!asyncLoad.isDone)
        {
            float elapsedTime = Time.time - loadStartTime;
            float progressValue = Mathf.Clamp01((asyncLoad.progress / 0.9f) * (elapsedTime / 5f));
            ProgressBar.value = progressValue;
            
            if (asyncLoad.progress >= 0.9f)
            {
                isSceneReady = true;
            }

            if (isSceneReady && elapsedTime >= 5f)
            {
                asyncLoad.allowSceneActivation = true;
                yield return new WaitForSeconds(0.5f);

                DynamicGI.UpdateEnvironment();
                loadingAnimator.SetTrigger("End");
                yield return new WaitForSeconds(1.5f);

                HideLoadingScreen();
            }

            yield return null;
        }
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}
