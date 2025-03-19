using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthManager : MonoBehaviour
{

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider manaSlider;
    public TMP_Text healthText;
    public TMP_Text manaText;
    [HideInInspector]public float maxHealth;
    [HideInInspector]public int maxMana;
    [HideInInspector]public float currentHealth;
    [HideInInspector]public float currentMana;

    private void Start()
    {
        // Khởi tạo PlayerStats
        GameManager.Instance.dataGameManager.playerStatsUsing.Initialize();
        LoadUI();

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
    public void LoadUI()
    {
        maxHealth = GameManager.Instance.dataGameManager.playerStatsUsing.maxHealth;
        maxMana = GameManager.Instance.dataGameManager.playerStatsUsing.maxMana;
        currentHealth = GameManager.Instance.dataGameManager.playerStatsUsing.currentHealth;
        currentMana = GameManager.Instance.dataGameManager.playerStatsUsing.currentMana;
    }

    // Hàm nhận sát thương
    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
    }

    // Hàm sử dụng mana
    public void UseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0f, maxMana);
    }

    // Cập nhật slider và text của sức khỏe
    public void UpdateHealthUI()
    {
        healthSlider.value = currentHealth / maxHealth;
        healthText.text = "Health: " + Mathf.Round(currentHealth).ToString() + " / " + Mathf.Round(maxHealth).ToString();
    }

    // Cập nhật slider và text của mana
    public void UpdateManaUI()
    {
        manaSlider.value = currentMana / maxMana;
        manaText.text = "Mana: " + Mathf.Round(currentMana).ToString() + " / " + Mathf.Round(maxMana).ToString();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExpOrb"))
        {
            UIExpOrb expOrb = other.GetComponent<UIExpOrb>();
            if (expOrb != null)
            {
                Destroy(other.gameObject); // Xóa viên EXP
            }
        }
    }
}
