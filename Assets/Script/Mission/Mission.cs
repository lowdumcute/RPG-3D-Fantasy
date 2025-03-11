using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Talk,   // Nhiệm vụ nói chuyện với NPC
    Kill,   // Nhiệm vụ tiêu diệt kẻ địch
    Collect // Nhiệm vụ thu thập vật phẩm
}

[CreateAssetMenu(fileName = "NewMission", menuName = "S0/Mission", order = 1)]
public class Mission : ScriptableObject
{
    [Header("Thông tin nhiệm vụ")]
    public string missionName;      // Tên nhiệm vụ
    [TextArea] public string description;  // Mô tả nhiệm vụ
    public MissionType missionType; // Loại nhiệm vụ
    public int requiredAmount;      // Số lượng yêu cầu để hoàn thành
    public bool isCompleted;        // Trạng thái hoàn thành nhiệm vụ

    [Header("Tiến độ nhiệm vụ")]
    public int currentProgress = 0; // Tiến trình hiện tại

    // Hàm tăng tiến độ nhiệm vụ
    public void UpdateProgress(int amount)
    {
        if (!isCompleted)
        {
            currentProgress += amount;
            if (currentProgress >= requiredAmount)
            {
                currentProgress = requiredAmount;
                isCompleted = true;
                Debug.Log($"Nhiệm vụ '{missionName}' đã hoàn thành!");
            }
        }
    }

    // Hàm reset nhiệm vụ
    public void ResetMission()
    {
        currentProgress = 0;
        isCompleted = false;
    }
}
