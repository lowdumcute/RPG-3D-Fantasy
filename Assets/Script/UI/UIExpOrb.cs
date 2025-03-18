using UnityEngine;

public class UIExpOrb : MonoBehaviour
{
    private Transform target; // Người chơi
    private float moveSpeed = 20f; // Tốc độ bay

    public void Setup(Transform player, int expAmount)
    {
        target = player;
        Invoke("DestroyExpOrb", 0.5f); // Xóa nếu không hút sau 5 giây
    }

    private void Update()
    {
        if (target != null)
        {
            // Bay về phía người chơi
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Khi chạm vào người chơi
        {
            Destroy(gameObject); // Xóa viên EXP
        }
    }

    private void DestroyExpOrb()
    {
        Destroy(gameObject);
    }
}
