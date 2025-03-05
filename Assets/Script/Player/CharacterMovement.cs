using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    private float acceleration = 5f; // Tốc độ tăng dần
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float jumpForce = 10f;
    private bool isRolling = false; // Biến kiểm tra trạng thái roll
    private float rollSpeed = 5f;  // Tốc độ của roll
    private float rollDuration = 0.5f; // Thời gian kéo dài roll

    [Header("Attack")]
    private int comboStep = 0;  // Để theo dõi bước combo
    private float attackCooldown = 0.01f;
    private float comboTimeLimit = 4f;  // Thời gian cho phép nhấn chuột liên tục để combo
    private float lastAttackTime = 0f;  // Thời gian của lần tấn công cuối
    private bool canAttack = true;  // Biến flag kiểm tra có thể tấn công hay không

    [Header("References")]
    
    private CharacterController controller;
    private Animator animator;
    private float downwardVelocity;
    [SerializeField] private bool isAttacking = false; // Biến kiểm tra xem nhân vật có đang tấn công hay không

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        animator.SetBool("Run", false);
    }

    void Update()
    {
        Move();
        HandleJump(); // Tách riêng kiểm tra và xử lý nhảy
        if (Input.GetMouseButtonDown(0))
        {
            downwardVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.C) && !isAttacking && !isRolling)  // Không thể roll khi đang tấn công hoặc đang roll
        {
            StartCoroutine(Roll());
        }

        // Kiểm tra thời gian giữa các lần tấn công để reset combo nếu cần
        if (Time.time - lastAttackTime > comboTimeLimit && comboStep > 0)
        {
            comboStep = 0;  // Reset combo nếu thời gian quá lâu
        }
    }

    private void Move()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && moveAmount > 0; // Giữ Shift để chạy
        bool isWalking = moveAmount > 0 && !isRunning; // Nếu không giữ Shift thì đi bộ

        if (isRunning)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);
            movementSpeed = GameManager.Instance.dataGameManager.playerStatsUsing.maxSpeed / 5f; // Tốc độ chạy
            CameraController.Instance.SetTargetZoom(0.7f); // Zoom gần khi chạy
        }
        else if (isWalking)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
            movementSpeed = GameManager.Instance.dataGameManager.playerStatsUsing.maxSpeed / 20f; // Tốc độ đi bộ
            CameraController.Instance.SetTargetZoom(0.9f); // Zoom vừa phải khi đi bộ
        }
        else
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            CameraController.Instance.SetTargetZoom(1f); // Trả lại zoom mặc định khi đứng yên
        }

        // Xoay theo hướng camera
        if (moveAmount > 0)
        {
            moveDirection = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z)) * moveDirection;
            var targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Tính toán vận tốc di chuyển
        if (isAttacking)
        {
            Vector3 velocity = moveDirection * movementSpeed/2f;
            // Áp dụng trọng lực nếu không chạm đất
            if (!controller.isGrounded)
            {
                downwardVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            }

            velocity.y = downwardVelocity;
            controller.Move(velocity * Time.deltaTime);
            }
        else
        {
        Vector3 velocity = moveDirection * movementSpeed;
    
        // Áp dụng trọng lực nếu không chạm đất
        if (!controller.isGrounded)
        {
            downwardVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        velocity.y = downwardVelocity;
        controller.Move(velocity * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            downwardVelocity = -2f; // Đặt giá trị thấp để tránh bị "dính" mặt đất

            if (Input.GetButtonDown("Jump"))
            {
                if (isAttacking || isRolling) return; // Không cho phép nhảy khi đang tấn công hoặc đang roll
                Jump();
            }
        }
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        comboStep =0; // Reset combo khi nhảy
        downwardVelocity = jumpForce; // Gán lực nhảy
    }

   private void Attack()
    {
        if (!canAttack) return;  // Nếu đang trong thời gian hồi chiêu thì không tấn công
        if (isAttacking || isRolling) return; // Không cho phép di chuyển khi đang tấn công hoặc đang roll

        isAttacking = true;
        canAttack = false;
        animator.SetBool("IsAttacking", true);
        lastAttackTime = Time.time;
        movementSpeed = 1f; // Tốc độ đi bộ

        // Tiến hành thực hiện combo
        comboStep++;

        // Kiểm tra combo
        if (comboStep > 3) comboStep = 1;  // Reset combo nếu quá 3 bước

        // Trigger animation dựa trên comboStep
        string attackTrigger = "Attack" + comboStep;
        
        animator.SetTrigger(attackTrigger);

        // Xác định hướng quay chỉ xoay trục Y
        Transform nearestEnemy = FindNearestEnemy(15f);
        if (nearestEnemy != null)
        {
            // Hướng về phía kẻ địch gần nhất (chỉ thay đổi góc Y)
            Vector3 directionToEnemy = (nearestEnemy.position - transform.position).normalized;
            float targetYRotation = Mathf.Atan2(directionToEnemy.x, directionToEnemy.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
        }
        else
        {
            // Hướng về phía camera nếu không có kẻ địch nào trong phạm vi (chỉ thay đổi góc Y)
            Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
            float cameraYRotation = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);
        }
    }
private Transform FindNearestEnemy(float radius)
{
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    Transform nearestEnemy = null;
    float minDistance = radius;

    foreach (GameObject enemy in enemies)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (distance < minDistance)
        {
            minDistance = distance;
            nearestEnemy = enemy.transform;
        }
    }

    return nearestEnemy;
}


    public void UnlockMovementAfterAttack()
    {
        animator.SetBool("IsAttacking", false);
        isAttacking = false; // Mở khóa di chuyển sau khi tấn công xong
        StartCoroutine(CooldownRoutine());  // Bắt đầu cooldown
    }
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private IEnumerator Roll()
    {
        isRolling = true;  // Đánh dấu nhân vật đang thực hiện roll
        animator.SetTrigger("Roll");

        // Tạm dừng các hành động di chuyển bình thường
        float rollTime = 0f;
        Vector3 rollDirection = transform.forward;  // Di chuyển theo hướng nhìn của nhân vật

        while (rollTime < rollDuration)
        {
            controller.Move(rollDirection * rollSpeed/3 * Time.deltaTime); // Di chuyển nhân vật theo roll
            rollTime += Time.deltaTime;
            downwardVelocity = -2f; 
            yield return null;
        }

        isRolling = false;  // Kết thúc roll
    }
}
