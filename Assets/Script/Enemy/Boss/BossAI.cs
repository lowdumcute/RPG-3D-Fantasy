using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("Thiết lập AI")]
    public Transform player;
    public float chaseRange = 10f; 
    public float attackRange = 2f; 
    public float cooldownTime = 3f; 

    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;
    public float obstacleCheckDistance = 1.5f;
    public float maxStepHeight = 1.0f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public float gravity = -9.8f;  

    private Animator animator;
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isOnCooldown = false;
    private bool isIdle = false; // Thêm trạng thái Idle
    private float lastAttackTime = -Mathf.Infinity; 
    [Header("Thanh máu")]
    [SerializeField] private GameObject healthBarUI;

    void Start()
    {
        healthBarUI.SetActive(false);
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Hiển thị thanh máu nếu player trong chaseRange, tắt nếu ra ngoài
        healthBarUI.SetActive(distanceToPlayer < chaseRange);

        // Nếu đang trong cooldown thì chỉ Idle và nhìn theo player
        if (isOnCooldown) 
        {
            if (Time.time - lastAttackTime > cooldownTime)
            {
                isOnCooldown = false;
                isIdle = false;
                isChasing = true;
            }
            else
            {
                isIdle = true;
                animator.SetBool("isRunning", false);
                LookAtPlayer();
                return;
            }
        }

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

        ApplyGravity();
    }

    void LateUpdate()
    {
        ApplyRootMotionMovement();
    }

    private void ApplyRootMotionMovement()
    {
        if (animator == null || controller == null) return;

        moveDirection = animator.deltaPosition;
        moveDirection.y = 0;

        if (isAttacking || isIdle)
        {
            controller.Move(Vector3.zero);
            return;
        }

        if (isChasing)
        {
            if (IsObstacleInFront())
            {
                animator.SetBool("isRunning", false);
                return;
            }

            LookAtPlayer();
        }

        controller.Move(moveDirection);
        KeepOnGround();
    }

    private void ApplyGravity()
    {
        if (isAttacking) return; // Không áp dụng trọng lực khi đang tấn công

        if (!controller.isGrounded)
        {
            moveDirection.y += gravity * Time.deltaTime;
            controller.Move(new Vector3(0, moveDirection.y - 3f, 0) * Time.deltaTime); // Đảm bảo nhân vật rơi xuống
        }
        else
        {
            moveDirection.y = 0; // Reset lại vận tốc rơi nếu chạm đất
        }
    }

    private void KeepOnGround()
    {
        if (isAttacking) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
            transform.position = hit.point;
            moveDirection.y = 0;
        }
    }

    private bool IsObstacleInFront()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(rayOrigin, direction, out hit, obstacleCheckDistance, obstacleLayer))
        {
            if (hit.point.y - transform.position.y > maxStepHeight)
            {
                return true;
            }
        }
        return false;
    }

    private void StartChase()
    {
        if (isChasing || isIdle) return;

        isChasing = true;
        isAttacking = false;
        animator.SetBool("isRunning", true);
    }

    private void StartAttack()
    {
        if (isAttacking || isOnCooldown) return;

        LookAtPlayer();

        isAttacking = true;
        isChasing = false;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Attack");

        lastAttackTime = Time.time;
    }

    public void EndAttack()
    {
        ApplyGravity();
        isOnCooldown = true;
        isAttacking = false;
        isIdle = true;
    }

    private void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;

        if (directionToPlayer.magnitude > 0f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (!isChasing && !isAttacking)
            {
                animator.SetBool("isRunning", false);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}