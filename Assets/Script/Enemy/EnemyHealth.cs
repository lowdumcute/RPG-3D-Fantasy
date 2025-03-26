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
    [SerializeField] private string TypeTarget;

    private Animator animator;
    private CharacterController controller;
    private AIMovement aiMovement;
    private Vector3 hitBackDirection;
    private float hitBackTimer;
    [SerializeField] private int exp;
    [SerializeField] GameObject expOrbPrefab; // Prefab viên kinh nghiệm
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
        if (aiMovement == null)
        {
            
        }
        else
        {
            aiMovement.enabled = false; // Tắt script di chuyển
        }
        animator.SetBool("isDead", true); // Chuyển sang trạng thái chết
        controller.enabled = false; // Tắt CharacterController
        MissionManager.Instance.IncreaseMissionProgress(TypeTarget, 1);
        PlayerLevel.Instance.GainExp(exp);
    }
    public void UnActive()
    {
        // Số viên EXP rơi ra
        int expDropCount = Random.Range(2, 5);

        for (int i = 0; i < expDropCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            GameObject expOrb = Instantiate(expOrbPrefab, transform.position + randomOffset, Quaternion.identity);

            // Thiết lập mục tiêu là người chơi
            expOrb.GetComponent<UIExpOrb>().Setup(GameObject.FindGameObjectWithTag("Player").transform, 10);
            gameObject.SetActive(false);
        }
    }
}
