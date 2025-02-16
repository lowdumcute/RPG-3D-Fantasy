using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{
    public float maxHealth = 100f;
    public float maxMana = 100f;
    public float maxAttack = 10f;
    public float maxSpeed = 5f;
    public float maxDefend = 5f;

    // Lưu giá trị hiện tại (thay đổi trong game)
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMana;

    // Hàm khởi tạo lại giá trị khi bắt đầu game
    public void Initialize()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
}
