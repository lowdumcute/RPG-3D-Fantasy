using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject attackColider;  // Collider của đòn tấn công
    public float radius = 1f;         // Bán kính tấn công
    public float attackRange = 2f;    // Khoảng cách tấn công
    public float attackDamage = 10f;  // Số sát thương
    public Transform playerTransform; // Transform của người chơi (để xác định hướng tấn công)

    void Start()
    {
        attackColider.SetActive(false); // Bắt đầu tắt attackCollider
    }

    void Update()
    {
        // Kiểm tra nếu người chơi nhấn nút tấn công (ví dụ: chuột trái)
        if (Input.GetMouseButtonDown(0))
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        // Lấy tất cả các collider trong phạm vi attackRange và bán kính radius
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in hitEnemies)
        {
            // Kiểm tra xem đối tượng có phải là quái vật không và có script EnemyHealth không
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Kiểm tra xem đối tượng có nằm trong hướng tấn công của người chơi hay không
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(playerTransform.forward, directionToEnemy);

                if (angle < 45f) // Chỉ tấn công đối tượng trong phạm vi góc 45 độ trước người chơi
                {
                    attackColider.SetActive(true);  // Bật attackCollider

                    // Gây sát thương cho kẻ thù
                    enemyHealth.TakeDamage(attackDamage);

                    StartCoroutine(AttackCooldown());
                }
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);  // Thời gian chờ giữa các lần tấn công
        attackColider.SetActive(false);  // Tắt attackCollider
    }
}
