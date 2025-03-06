using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance;
    [SerializeField] private GameObject Settingmenu;
    [SerializeField] public GameObject Player;
    public bool isActive;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Đảm bảo chỉ có 1 instance tồn tại
            return;
        }
    }
    public void Start()
    {
        Player.transform.position = GameManager.Instance.dataGameManager.Position;
        isActive = false;
        Settingmenu.SetActive(isActive);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            SettingMenu();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            SettingMenu();
        }
    }
    public void SaveGame()
    {
        GameManager.Instance.SaveProgress();
    }
    public void SettingMenu()
    {

        isActive = !isActive;
        Settingmenu.SetActive(isActive);

        if (isActive)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
    public void TurnOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraController.isPaused = false; // Bật lại camera
        isActive=false;
    }
    public void TurnOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraController.isPaused = true; // Tắt camera
        isActive= true;
    }
}
