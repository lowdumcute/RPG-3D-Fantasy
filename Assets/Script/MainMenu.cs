using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("ChooseCharacterScene");
        GameManager.Instance.dataGameManager.Position = new Vector3(548, 11, 375);
        DynamicGI.UpdateEnvironment(); // Cập nhật ánh sáng toàn cục
    }

    public void LoadGame(string sceneName)
    {
        StartCoroutine(LoadSceneAndWait(sceneName));
    }

    private IEnumerator LoadSceneAndWait(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null; // Chờ Scene load hoàn tất
        }

        yield return new WaitForSeconds(0.1f); // Đợi thêm chút để đảm bảo mọi thứ đã sẵn sàng
        GameManager.Instance.LoadProgress(); // Gọi LoadProgress() sau khi Scene đã load xong
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
