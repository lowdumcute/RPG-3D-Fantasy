using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public static PlayerLevel Instance; // Singleton

    public event Action<int> OnLevelUp; // Sự kiện khi lên cấp

    [SerializeField] private int currentLevel ; // Level hiện tại
    [SerializeField] private int currentExp ; // EXP hiện tại
    [SerializeField] private int expToNextLevel = 100; // EXP cần để lên level tiếp theo

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
        currentLevel = GamePlayManager.Instance.dataGameManager.currentLevel;
        currentExp = GamePlayManager.Instance.dataGameManager.exp;
    }

    // Hàm để tăng EXP
    public void GainExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"Nhận {amount} EXP. Tổng: {currentExp}/{expToNextLevel}");

        // Kiểm tra xem có đủ EXP để lên cấp không
        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    // Hàm xử lý lên cấp
    private void LevelUp()
    {
        currentExp -= expToNextLevel; // Giữ lại EXP dư
        currentLevel++; // Tăng level
        expToNextLevel += currentLevel * 50; // Tăng EXP yêu cầu mỗi lần lên cấp

        Debug.Log($" Level Up! Level hiện tại: {currentLevel}");

        // Gọi sự kiện OnLevelUp
        OnLevelUp?.Invoke(currentLevel); 
    }

    public int GetLevel() => currentLevel;
    public int GetExp() => currentExp;
    public int GetExpToNextLevel() => expToNextLevel;
}
