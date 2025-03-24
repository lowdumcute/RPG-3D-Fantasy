using System.Collections;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private GameObject VFX;
    [SerializeField] private GameObject VFXPoint; // vị trí spawn 
    public Transform player; // Mục tiêu người chơi
    public float jumpHeight = 5f; // Độ cao nhảy
    public float jumpDuration = 1f; // Thời gian nhảy
    public float attackDuration = 0.5f; // Thời gian đứng yên khi chạm đất
    public float damageAmount = 10f; // Lượng sát thương
    public float jumpSpeed = 10f; // Tốc độ nhảy tới mục tiêu
    public float gravity = -20f; // Trọng lực mạnh hơn giúp rơi xuống nhanh hơn
    public Collider attackCollider; // Box Collider dùng để tấn công

    private Vector3 targetPosition; // Điểm đến khi nhảy
    private CharacterController controller;
    private Vector3 startPosition; // Lưu vị trí ban đầu

    void Start()
    {
        attackCollider.enabled = false; // Tắt collider ban đầu
        controller = GetComponent<CharacterController>();
        startPosition = transform.position; // Lưu lại vị trí ban đầu
    }

    // Hàm bắt đầu nhảy tấn công
    public void JumpToAttack()
    {
        StartCoroutine(PerformJumpAttack());
    }

    private IEnumerator PerformJumpAttack()
    {
        startPosition = transform.position; // Cập nhật vị trí ban đầu của mỗi lần nhảy
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        targetPosition = player.position - directionToPlayer * 1f; // Cách người chơi 1 đơn vị

        float timer = 0f;

        // Giai đoạn nhảy lên và di chuyển ngang
        while (timer < jumpDuration)
        {
            float t = timer / jumpDuration;
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight; // Quỹ đạo nhảy parabolic
            Vector3 horizontalMove = Vector3.Lerp(startPosition, targetPosition, t); // Tiến đến mục tiêu

            // Áp dụng di chuyển
            Vector3 moveDirection = new Vector3(horizontalMove.x, startPosition.y + height, horizontalMove.z) - transform.position;
            controller.Move(moveDirection);

            timer += Time.deltaTime;
            yield return null;
        }

        // Giai đoạn rơi xuống
        while (!controller.isGrounded)
        {
            controller.Move(Vector3.down * Mathf.Abs(gravity) * Time.deltaTime);
            yield return null;
        }

        // Chờ attackDuration trước khi reset trạng thái
        yield return new WaitForSeconds(attackDuration);
    }
    public void OnEnableCollider()
    {
        SpawnVFX();
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }
    private void SpawnVFX()
    {
        if (VFX != null)
        {
            Instantiate(VFX, transform); // Spawn VFX và đặt làm con của object này
        }
    }
}
