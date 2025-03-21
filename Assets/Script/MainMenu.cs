using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneChangeManager.Instance.LoadScene("ChooseCharacterScene");
        GameManager.Instance.dataGameManager.Position = new Vector3(548, 11, 375);
    }

    public void LoadGame(string sceneName)
    {
        GameManager.Instance.LoadProgress(); // Gọi LoadProgress() sau khi Scene đã load xong
        SceneChangeManager.Instance.LoadScene(sceneName);
    }


    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
