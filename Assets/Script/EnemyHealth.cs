using UnityEngine;
using UnityEngine.UI; // Để làm việc với UI

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Máu tối đa của địch
    private float currentHealth; // Máu hiện tại của địch
    Animator animator; // Animator của địch
    private void Start()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator
        currentHealth = maxHealth; // Thiết lập máu ban đầu

        // Nếu có Slider trong UI, thiết lập giá trị ban đầu
        if (healthSlider != null)
        {
            healthSlider.maxValue = 1f; // Đặt giá trị tối đa của Slider là 1 (100%)
            healthSlider.value = 1f; // Đặt giá trị ban đầu của Slider là 100%
        }
    }

    public Slider healthSlider; // Slider hiển thị máu

    // Hàm này sẽ được gọi khi đối tượng bị nhận sát thương
    public void TakeDamage(float damage)
    {
        animator.SetTrigger("Hit"); 
        currentHealth -= damage; // Giảm máu của địch

        // Cập nhật giá trị của Slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth; // Tính toán tỷ lệ phần trăm máu còn lại
        }

        // Nếu máu còn lại nhỏ hơn hoặc bằng 0, gọi hàm Die()
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hàm này sẽ được gọi khi địch chết
    private void Die()
    {
        // Gọi animation chết hoặc bất kỳ hiệu ứng gì khi địch chết
        Debug.Log("Enemy died!");
        // Hủy đối tượng hoặc tắt hoạt động của đối tượng
        Destroy(gameObject); // Xóa đối tượng khỏi scene
    }

}
