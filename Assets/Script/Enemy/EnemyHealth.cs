using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthSlider;
    public float hitBackForce = 3f; // Lực đẩy khi bị đánh
    public float hitBackDuration = 0.2f; // Thời gian đẩy lùi
    public float rotationSpeed = 10f;

    private Animator animator;
    private CharacterController controller;
    private AIMovement aiMovement;
    private Vector3 hitBackDirection;
    private float hitBackTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        aiMovement = GetComponent<AIMovement>(); // Lấy script di chuyển
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
    }

    public void TakeDamage(float damage, Vector3 attackPosition)
    {
        if(damage>=30)
        {
            animator.SetTrigger("Hit");
        }

        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        // Dừng di chuyển
        if (aiMovement != null)
        {
            aiMovement.isWalking = false; // Dừng di chuyển
            animator.SetBool("isRunning", false);
        }

        // Xác định hướng hit back (ngược với hướng người chơi)
        Vector3 directionToAttacker = (transform.position - attackPosition).normalized;
        hitBackDirection = directionToAttacker; // Gán hướng đẩy lùi
        hitBackTimer = hitBackDuration;

        // Xoay Enemy chỉ theo trục Y về hướng người chơi
        Quaternion lookRotation = Quaternion.LookRotation(directionToAttacker);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y -180, 0);

        // Sau một thời gian, tiếp tục di chuyển
        if (aiMovement != null)
        {
            StartCoroutine(ResumeMovement(1f)); // Dừng trong 1 giây rồi đi tiếp
        }

        // Kiểm tra nếu enemy chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator ResumeMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (aiMovement != null)
        {
            aiMovement.ChooseDirection(); // Bắt đầu di chuyển lại
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
