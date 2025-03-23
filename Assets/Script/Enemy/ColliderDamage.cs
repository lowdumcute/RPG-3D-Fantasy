using UnityEngine;
using System.Collections;

public class ColliderDamage : MonoBehaviour
{
    public float damageAmount = 10f; // Số lượng sát thương
    private bool isColliderActive = false; // Kiểm tra trạng thái của collider

    private void OnEnable()
    {
        isColliderActive = true; // Bật trạng thái kiểm tra va chạm khi script được kích hoạt

        StartCoroutine(CheckForCollisionsDelayed());
    }

    private void OnDisable()
    {
        isColliderActive = false; // Ngừng kiểm tra va chạm khi script bị tắt
    }

    private IEnumerator CheckForCollisionsDelayed()
    {
        // Chờ 0.1 giây rồi mới kiểm tra va chạm
        yield return new WaitForSeconds(0.1f);

        // Kiểm tra va chạm với đối tượng Player ngay sau khi script được bật
        if (isColliderActive)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // Kiểm tra tất cả đối tượng trong phạm vi 1m
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Debug.Log("Player detected immediately, dealing damage.");
                    PlayerHealthManager playerHealth = hitCollider.GetComponent<PlayerHealthManager>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageAmount);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliderActive && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the collider, dealing damage.");
            PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
