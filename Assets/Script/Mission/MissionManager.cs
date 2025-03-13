using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance; // Singleton

    [SerializeField] private GameObject missionPrefab; // Prefab của mỗi nhiệm vụ
    [SerializeField] private Transform missionContainer; // Vị trí hiển thị danh sách nhiệm vụ
    [SerializeField] private List<Mission> allMission = new List<Mission>(); // Danh sách tất cả nhiệm vụ
    private Dictionary<Mission, GameObject> missionObjects = new Dictionary<Mission, GameObject>(); // Lưu trữ GameObject theo Mission

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateMissions();
    }

    public void AddMission(Mission mission)
    {
        if (!allMission.Contains(mission))
        {
            allMission.Add(mission);
            mission.isActive = true;
            Debug.Log($"Đã nhận nhiệm vụ: {mission.missionName}");
        }
        else
        {
            mission.isActive = true;
            Debug.Log($"Nhiệm vụ {mission.missionName} đã được kích hoạt!");
        }

        UpdateMissions();
    }

    public void UpdateMissionProgress(MissionType type, int amount)
    {
        foreach (var mission in allMission)
        {
            if (mission.missionType == type && !mission.isCompleted)
            {
                mission.UpdateProgress(amount);
                Debug.Log($"Cập nhật nhiệm vụ: {mission.missionName} ({mission.currentProgress}/{mission.requiredAmount})");
                UpdateMissions();
            }
        }
    }

    public void CompleteMission(Mission mission)
    {
        if (mission.isCompleted)
        {
            Debug.Log($"Nhiệm vụ '{mission.missionName}' đã hoàn thành!");
            UpdateMissions();
        }
    }

    public void UpdateMissions()
    {
        foreach (Mission mission in allMission)
        {
            if (!mission.isActive || mission.isCompleted) continue;

            if (!missionObjects.ContainsKey(mission))
            {
                GameObject missionUI = Instantiate(missionPrefab, missionContainer);
                missionObjects[mission] = missionUI;

                // Lấy các TMP Text theo thứ tự con trong Prefab
                TMP_Text nameText = missionUI.transform.GetChild(0).GetComponent<TMP_Text>();
                TMP_Text progressText = missionUI.transform.GetChild(1).GetComponent<TMP_Text>();

                nameText.text = mission.missionName;
                progressText.text = $"{mission.currentProgress}/{mission.requiredAmount}";
            }
            else
            {
                // Nếu đã tồn tại, chỉ cần cập nhật tiến trình
                TMP_Text progressText = missionObjects[mission].transform.GetChild(1).GetComponent<TMP_Text>();
                progressText.text = $"{mission.currentProgress}/{mission.requiredAmount}";
            }
        }
    }
}
