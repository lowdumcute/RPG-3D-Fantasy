using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Talk,   // Nhiệm vụ nói chuyện với NPC
    Kill,   // Nhiệm vụ tiêu diệt kẻ địch
    Collect // Nhiệm vụ thu thập vật phẩm
}

[CreateAssetMenu(fileName = "NewMission", menuName = "ScriptableObject/Mission", order = 1)]
public class Mission : ScriptableObject
{
    [Header("Thông tin nhiệm vụ")]
    public string missionName;      // Tên nhiệm vụ
    [TextArea] public string description;  // Mô tả nhiệm vụ
    public string targetName;       // Tên mục tiêu
    public MissionType missionType; // Loại nhiệm vụ
    [Header("Trạng thái nhiệm vụ")]
    public bool isReceive; // nhiệm vụ đã nhận thưởng chưa
    public bool isActive; // nhiệm vụ được kích hoạt chưa 
    public bool isCompleted;        // Trạng thái hoàn thành nhiệm vụ

    [Header("Tiến độ nhiệm vụ")]
    public int currentProgress = 0; // Tiến trình hiện tại
    public int requiredAmount;      // Số lượng yêu cầu để hoàn thành

    [Header("Phần thưởng")]
    public int rewardExp;           // Số kinh nghiệm thưởng
    public int rewardGold;          // Số vàng thưởng
    [Header("đối thoại nhiệm vụ")]
    [TextArea(2, 5)] public string AskedMissionDialogue; // Đối thoại khi hỏi nhiệm vụ
    [TextArea(2, 5)] public string AcceptMissionDialogue; // Đối thoại khi nhận nhiệm vụ
    [TextArea(2, 5)] public string CompleteMissionDialogue; // Đối thoại khi hoàn thành nhiệm vụ

    // Hàm tăng tiến độ nhiệm vụ
    public void UpdateProgress(int amount)
    {
        if (isCompleted) return; // Nếu đã hoàn thành thì không cần cập nhật nữa

        if (requiredAmount <= 0)
        {
            Debug.LogWarning($"Nhiệm vụ '{missionName}' có số lượng yêu cầu không hợp lệ: {requiredAmount}");
            return;
        }

        currentProgress += amount;
        Debug.Log($"Tiến trình nhiệm vụ '{missionName}': {currentProgress}/{requiredAmount}");

        if (currentProgress >= requiredAmount)
        {
            currentProgress = requiredAmount;
            isCompleted = true;
            Debug.Log($" Nhiệm vụ '{missionName}' đã hoàn thành!");
        }
    }

    // Hàm reset nhiệm vụ
    public void ResetMission()
    {
        currentProgress = 0;
        isCompleted = false;
    }
}
