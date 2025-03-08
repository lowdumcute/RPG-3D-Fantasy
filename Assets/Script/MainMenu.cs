using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void NewGame()
    {
        SceneManager.LoadScene("ChooseCharacterScene");
        GameManager.Instance.dataGameManager.Position = new Vector3(548, 11, 375);
        DynamicGI.UpdateEnvironment(); // Cập nhật ánh sáng toàn cục
    }
    public void LoadGame(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        GameManager.Instance.LoadProgress();
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
