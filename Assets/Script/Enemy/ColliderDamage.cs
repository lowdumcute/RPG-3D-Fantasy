using UnityEngine;
using System.Collections;

public class ColliderDamage : MonoBehaviour
{
    public float damageAmount = 10f; // Số lượng sát thương
    private bool isColliderActive = false; // Kiểm tra trạng thái của collider

    private void OnEnable()
    {
        isColliderActive = true; // Bật trạng thái kiểm tra va chạm khi script được kích hoạt

    }

    private void OnDisable()
    {
        isColliderActive = false; // Ngừng kiểm tra va chạm khi script bị tắt
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Gây 10 damage
            }
        }
    }
}
