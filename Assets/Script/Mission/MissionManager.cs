using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance; // Singleton

    [SerializeField] public List<Mission> allMission = new List<Mission>(); // Danh sách tất cả nhiệm vụ 
    [SerializeField] private Dictionary<Mission, GameObject> missionObjects = new Dictionary<Mission, GameObject>(); // Lưu trữ GameObject theo Mission
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        UpdateMissions();
    }

    public void AddMission(Mission mission)
    {
        if (allMission.Contains(mission))
        {
            // Nếu mission đã tồn tại, kích hoạt nó
            mission.isActive = true;
            Debug.Log($"Nhiệm vụ {mission.missionName} đã được kích hoạt!");
        }
        else
        {
            // Nếu mission chưa có, thêm vào danh sách và kích hoạt
            mission.isActive = true;
            allMission.Add(mission);
            Debug.Log($"Đã nhận nhiệm vụ: {mission.missionName}");
        }

        UpdateMissions(); // Cập nhật lại trạng thái hiển thị của các nhiệm vụ
    }

    public void UpdateMissionProgress(MissionType type, int amount)
    {
        foreach (var mission in allMission)
        {
            if (mission.missionType == type && !mission.isCompleted)
            {
                mission.UpdateProgress(amount);
                Debug.Log($"Cập nhật tiến trình nhiệm vụ: {mission.missionName} ({mission.currentProgress}/{mission.requiredAmount})");
            }
        }
    }

    public void CompleteMission(Mission mission)
    {
        if (mission.isCompleted)
        {
            Debug.Log($"Nhiệm vụ '{mission.missionName}' đã hoàn thành!");
        }
    }
    public void UpdateMissions()
    {
        foreach (Mission mission in allMission)
        {
            if (missionObjects.ContainsKey(mission))
            {
                // Nếu mission đã có object, chỉ cần cập nhật hiển thị
                missionObjects[mission].SetActive(mission.isActive && !mission.isCompleted);
            }
            else
            {
                // Nếu mission chưa có object, tạo mới
                GameObject missionObject = new GameObject(mission.missionName);
                missionObject.transform.SetParent(transform); // Gán làm con của MissionManager
                missionObjects[mission] = missionObject;
                missionObject.SetActive(mission.isActive && !mission.isCompleted);
            }
        }
    }
}
