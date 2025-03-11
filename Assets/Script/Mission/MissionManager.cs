using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance; // Singleton

    [SerializeField] private List<Mission> activeMissions = new List<Mission>(); // Danh sách nhiệm vụ đang làm

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddMission(Mission mission)
    {
        if (!activeMissions.Contains(mission))
        {
            activeMissions.Add(mission);
            Debug.Log($"Đã nhận nhiệm vụ: {mission.missionName}");
        }
    }

    public void UpdateMissionProgress(MissionType type, int amount)
    {
        foreach (var mission in activeMissions)
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
}
