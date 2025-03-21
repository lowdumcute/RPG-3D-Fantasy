using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void Change(string NameScene)
    {
        GameManager.Instance.dataGameManager.Position = new Vector3(548, 11, 375);
        SceneChangeManager.Instance.LoadScene(NameScene);
    }
}
