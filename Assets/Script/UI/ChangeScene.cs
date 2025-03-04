using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Change(string NameScene)
    {
        SceneManager.LoadScene(NameScene);
        DynamicGI.UpdateEnvironment(); // Cập nhật ánh sáng toàn cục
    }
}
