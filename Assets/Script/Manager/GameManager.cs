using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public DataGameManager dataGameManager;
    public static GameManager Instance { get; private set; } // Singleton
    private void Awake()
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
    void Start()
    {   
        // Dùng dữ liệu trong dataGameManager để tiếp tục game
        Debug.Log("Current Level: " + dataGameManager.currentLevel);
    }
    // Lưu dữ liệu vào file JSON
    public void SaveProgress()
    {
        GameData data = new GameData();
        data.level = dataGameManager.currentLevel; // lưu cấp độ
        data.Role = dataGameManager.playerStatsUsing.NameRole; // lưu tên role
        data.position= GamePlayManager.Instance.Player.transform.position; // lưu vị trí
        data.SceneSave = SceneManager.GetActiveScene().name; //

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savegame.json", json);
        Debug.Log ("Đã lưu" + dataGameManager);
    }

    public void LoadProgress()
    {
        string filePath = Application.persistentDataPath + "/savegame.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            
            // Cập nhật ScriptableObject với dữ liệu từ JSON
            dataGameManager.currentLevel = data.level;
            dataGameManager.Position = data.position;
            foreach (var role in dataGameManager.AllRoleStats)
            {
            if (role.name == data.Role) // So sánh với tên đã lưu
            {
                dataGameManager.playerStatsUsing = role;
                break;
            }
            }
        }
        else
        {
            // Nếu không có file lưu, khởi tạo với giá trị mặc định (ví dụ, cấp độ 1)
            dataGameManager.currentLevel = 1;
            GamePlayManager.Instance.Player.GetComponent<CharacterController>().enabled = true;
        }
    }
    public void AddPlayerStats(PlayerStats playerStats)
    {
        dataGameManager.playerStatsUsing = playerStats;
    }
}
