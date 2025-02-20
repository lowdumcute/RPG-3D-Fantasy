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
    [SerializeField] AIMovement aIMovement;

    private Animator animator;
    private CharacterController controller;
    private Vector3 hitBackDirection;
    private float hitBackTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
    }

    private void Update()
    {
        // Xử lý hit back (nếu đang bị đẩy lùi)
        if (hitBackTimer > 0)
        {
            controller.Move(hitBackDirection * hitBackForce * Time.deltaTime);
            hitBackTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage, Vector3 attackPosition)
    {
        aIMovement.enabled = false;
        animator.SetTrigger("Hit");
        currentHealth -= damage;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        // Quay enemy về hướng bị tấn công
        Vector3 directionToAttacker = transform.position - attackPosition;
        directionToAttacker.y = 0; // Không quay theo trục Y
        Quaternion lookRotation = Quaternion.LookRotation(directionToAttacker);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Thiết lập hit back
        hitBackDirection = directionToAttacker.normalized;
        hitBackTimer = hitBackDuration;

        // Kiểm tra nếu enemy chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
