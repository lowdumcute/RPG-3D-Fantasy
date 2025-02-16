using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Player Stats")]
    public PlayerStats playerStats;  // Tham chiếu tới PlayerStats

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider manaSlider;
    public TMP_Text healthText;
    public TMP_Text manaText;

    private void Start()
    {
        // Khởi tạo PlayerStats
        playerStats.Initialize();

        // Cập nhật UI ban đầu
        UpdateHealthUI();
        UpdateManaUI();
    }

    private void Update()
    {
        // Giả lập giảm sức khỏe và mana (ví dụ: trong game thực tế bạn sẽ cập nhật giá trị này khi có sự kiện)
        if (Input.GetKeyDown(KeyCode.H)) // Giảm máu khi nhấn H
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.M)) // Giảm mana khi nhấn M
        {
            UseMana(10f);
        }

        // Cập nhật lại UI khi có thay đổi
        UpdateHealthUI();
        UpdateManaUI();
    }

    // Hàm nhận sát thương
    public void TakeDamage(float amount)
    {
        playerStats.currentHealth = Mathf.Clamp(playerStats.currentHealth - amount, 0f, playerStats.maxHealth);
    }

    // Hàm sử dụng mana
    public void UseMana(float amount)
    {
        playerStats.currentMana = Mathf.Clamp(playerStats.currentMana - amount, 0f, playerStats.maxMana);
    }

    // Cập nhật slider và text của sức khỏe
    private void UpdateHealthUI()
    {
        healthSlider.value = playerStats.currentHealth / playerStats.maxHealth;
        healthText.text = "Health: " + Mathf.Round(playerStats.currentHealth).ToString() + " / " + Mathf.Round(playerStats.maxHealth).ToString();
    }

    // Cập nhật slider và text của mana
    private void UpdateManaUI()
    {
        manaSlider.value = playerStats.currentMana / playerStats.maxMana;
        manaText.text = "Mana: " + Mathf.Round(playerStats.currentMana).ToString() + " / " + Mathf.Round(playerStats.maxMana).ToString();
    }
}
