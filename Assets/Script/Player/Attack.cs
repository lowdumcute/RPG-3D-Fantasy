using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider;  // Collider của đòn tấn công
    [SerializeField] private GameObject VFX;       // Prefab VFX khi đánh trúng
    [SerializeField] private float attackDamage = 10f;  // Số sát thương

    void Start()
    {
        VFX.SetActive(false);
    }

    public void AttackMelee()
{
    // Bật Attack Collider
    attackCollider.enabled = true;

    // Lấy tất cả đối tượng trong phạm vi AttackCollider
    Collider[] hitEnemies = Physics.OverlapBox(attackCollider.bounds.center, attackCollider.bounds.extents, attackCollider.transform.rotation);

    foreach (Collider enemy in hitEnemies)
    {
        // Kiểm tra xem đối tượng có CharacterController hoặc tag "Enemy"
        if (enemy.TryGetComponent<CharacterController>(out CharacterController enemyController) || enemy.CompareTag("Enemy"))
        {
            // Kiểm tra xem có EnemyHealth không
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                // Gây sát thương
                enemyHealth.TakeDamage(attackDamage, transform.position);

                // Di chuyển VFX đến vị trí enemy trúng đòn
                VFX.transform.position = enemy.transform.position + Vector3.up * 1f;
                VFX.SetActive(true); // Bật VFX
                StartCoroutine(DisableVFX());
            }
        }
    }
}
private IEnumerator DisableVFX()
{
    yield return new WaitForSeconds(0.5f); // VFX tồn tại trong 1 giây
    VFX.SetActive(false); // Tắt VFX
}


public void  DisableAttackCollider()
{
    attackCollider.enabled = false;
}
}
