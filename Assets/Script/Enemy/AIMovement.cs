using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Movement and Animation")]
    private Animator animator;
    private CharacterController controller;

    public float moveSpeed = 0.2f;
    public float gravity = -9.81f;  // Trọng lực
    public float jumpHeight = 1f;   // Chiều cao nhảy (nếu cần thiết)

    // [Header] - Các biến cho Combat
    [Header("Combat")]
    private Transform player;
    public float detectionRange = 5f;  // Cự ly phát hiện
    public float attackRange = 1.5f;   // Cự ly tấn công
    public float attackCooldown = 1f;  // Thời gian giữa các đòn tấn công
    private float lastAttackTime;
    public Collider attackCollider; // Box Collider dùng để tấn công

    // [Header] - Các biến cho Patrol và Movement
    [Header("Patrol and Random Movement")]
    Vector3 stopPosition;
    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    int WalkDirection;
    public bool isWalking;

    private Vector3 velocity;  // Lưu trữ tốc độ của đối tượng để áp dụng gravity

    // Khởi tạo các thành phần
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = walkTime;

        attackCollider.enabled = false; // Tắt collider ban đầu

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        ChooseDirection();
    }

    void Update()
    {
        ApplyGravity(); 
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                Attack();
                return;
            }
            else if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer();
                return;
            }
        }

        Patrol();
    }

    // Phương thức di chuyển tuần tra (Patrol)
    void Patrol()
    {
        if (isWalking)
        {
            animator.SetBool("isRunning", true);
            walkCounter -= Time.deltaTime;

            Vector3 moveDirection = Vector3.zero;

            switch (WalkDirection)
            {
                case 0: moveDirection = transform.forward; break;
                case 1: transform.localRotation = Quaternion.Euler(0f, 90, 0f); moveDirection = transform.forward; break;
                case 2: transform.localRotation = Quaternion.Euler(0f, -90, 0f); moveDirection = transform.forward; break;
                case 3: transform.localRotation = Quaternion.Euler(0f, 180, 0f); moveDirection = transform.forward; break;
            }

            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            if (walkCounter <= 0)
            {
                isWalking = false;
                animator.SetBool("isRunning", false);
                waitCounter = waitTime;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0) ChooseDirection();
        }
    }

    // Phương thức di chuyển để theo dõi player
    void ChasePlayer()
    {
        isWalking = false;
        animator.SetBool("isRunning", true);
        
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Giữ nguyên trục Y

        transform.rotation = Quaternion.LookRotation(direction);
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    // Phương thức tấn công
    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            isWalking = false;
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    // Các phương thức để kích hoạt và vô hiệu hóa collider tấn công
    public void OnEnableCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        isWalking = true;
        attackCollider.enabled = false;
    }

    // Phương thức chọn hướng đi ngẫu nhiên
    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    // Thêm gravity vào CharacterController
    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;  // Gravity được áp dụng vào velocity.y
        }
        else
        {
            if (velocity.y < 0)  // Ngừng ảnh hưởng của gravity khi nhân vật chạm đất
            {
                velocity.y = 0f;
            }
        }

        controller.Move(velocity * Time.deltaTime);  // Áp dụng velocity cho CharacterController
    }
}
