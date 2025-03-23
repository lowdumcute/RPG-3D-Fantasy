using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance;
    public GameObject loadingScreen;
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

        float loadStartTime = Time.time; // Lưu thời điểm bắt đầu
        bool isSceneReady = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                isSceneReady = true;
            }

            // Chờ ít nhất 9 giây hoặc lâu hơn nếu scene chưa sẵn sàng
            if (isSceneReady && Time.time - loadStartTime >= 5f)
            {
                asyncLoad.allowSceneActivation = true; // Chuyển sang scene mới ngay lập tức
                yield return new WaitForSeconds(0.5f); // Chờ một chút để scene hoàn toàn chuyển đổi

                DynamicGI.UpdateEnvironment(); // Cập nhật ánh sáng toàn cục
                loadingAnimator.SetTrigger("End"); // Kích hoạt animation kết thúc
                yield return new WaitForSeconds(1.5f); // Đợi animation kết thúc

                HideLoadingScreen(); // Ẩn màn hình loading
            }

            yield return null;
        }
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}
