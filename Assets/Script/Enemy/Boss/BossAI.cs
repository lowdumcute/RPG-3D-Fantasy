using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("Thiết lập AI")]
    public Transform player; // Người chơi cần đuổi theo
    public float chaseRange = 10f; // Khoảng cách phát hiện người chơi
    public float attackRange = 2f; // Khoảng cách tấn công
    public float patrolSpeed = 1f; // Tốc độ tuần tra
    public float chaseSpeed = 2f; // Tốc độ đuổi theo
    public float obstacleCheckDistance = 1.5f; // Khoảng cách kiểm tra vật cản
    public float maxStepHeight = 1.0f; // Chiều cao tối đa boss có thể bước qua
    public LayerMask groundLayer;
    public LayerMask obstacleLayer; // Layer kiểm tra vật cản

    private Animator animator;
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isChasing = false;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer < attackRange)
        {
            StartAttack();
        }
        else if (distanceToPlayer < chaseRange)
        {
            StartChase();
        }
        else
        {
            isChasing = false;
        }
    }

    void LateUpdate()
    {
        ApplyRootMotionMovement();
    }

    // Xử lý di chuyển dựa trên Root Motion
    private void ApplyRootMotionMovement()
    {
        if (animator == null || controller == null) return;

        moveDirection = animator.deltaPosition; // Lấy dữ liệu từ Root Motion
        moveDirection.y = 0; // Giữ boss trên mặt đất

        // Nếu đang đuổi theo thì kiểm tra vật cản trước mặt
        if (isChasing)
        {
            if (IsObstacleInFront()) 
            {
                animator.SetBool("isRunning", false);
                return;
            }

            // Xoay về phía người chơi khi di chuyển
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0; // Không xoay theo trục Y
            if (directionToPlayer.magnitude > 0f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        controller.Move(moveDirection); // Di chuyển với CharacterController
        KeepOnGround();
    }

    // Giữ boss luôn trên mặt đất
    private void KeepOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
            Vector3 newPos = hit.point;
            transform.position = newPos;
        }
    }

    // Kiểm tra vật cản trước mặt
    private bool IsObstacleInFront()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f; // Bắt đầu từ ngang tầm nhân vật
        Vector3 direction = transform.forward;

        if (Physics.Raycast(rayOrigin, direction, out hit, obstacleCheckDistance, obstacleLayer))
        {
            // Kiểm tra nếu vật cản cao hơn mức có thể bước qua
            if (hit.point.y - transform.position.y > maxStepHeight)
            {
                return true; // Có vật cản không thể vượt qua
            }
        }
        return false;
    }

    // Bắt đầu đuổi theo người chơi
    private void StartChase()
    {
        if (isChasing) return;

        isChasing = true;
        isAttacking = false;
        animator.SetBool("isRunning", true);
    }

    // Bắt đầu tấn công
    private void StartAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        isChasing = false;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Attack");
    }

    // Tuần tra khi không có người chơi gần đó
    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (!isChasing && !isAttacking)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isRunning", false);
            }
            yield return new WaitForSeconds(5f);
        }
    }
}
