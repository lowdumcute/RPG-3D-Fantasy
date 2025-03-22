using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("Thiết lập AI")]
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;
    public float obstacleCheckDistance = 1.5f;
    public float maxStepHeight = 1.0f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

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

    private void ApplyRootMotionMovement()
    {
        if (animator == null || controller == null) return;

        moveDirection = animator.deltaPosition;
        moveDirection.y = 0; // Không bị ảnh hưởng bởi trọng lực gốc

        if (isAttacking)
        {
            controller.Move(moveDirection); // Giữ nguyên Root Motion khi Attack
            return;
        }

        if (isChasing)
        {
            if (IsObstacleInFront()) 
            {
                animator.SetBool("isRunning", false);
                return;
            }

            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            if (directionToPlayer.magnitude > 0f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        controller.Move(moveDirection);
        KeepOnGround();
    }

    private void KeepOnGround()
    {
        if (isAttacking) return; // Không áp dụng trọng lực khi Attack

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
            Vector3 newPos = hit.point;
            transform.position = newPos;
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
        if (isChasing) return;

        isChasing = true;
        isAttacking = false;
        animator.SetBool("isRunning", true);
    }

    private void StartAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        isChasing = false;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Attack");
    }

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
