using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    private float currentSpeed ;
    private float acceleration = 5f; // Tốc độ tăng dần
    private float deceleration = 10f; // Tốc độ giảm dần
    [SerializeField] private float movementSpeed = 5f;
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
        movementSpeed = GameManager.Instance.dataGameManager.playerStatsUsing.maxSpeed ; // Tốc độ hiện tại
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
        if (Input.GetKeyDown("left shift") && !isAttacking && !isRolling)  // Không thể roll khi đang tấn công hoặc đang roll
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
        if (isAttacking) return; // Không cho phép di chuyển khi đang tấn công
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveAmount > 0)
        {
            // Tăng tốc dần
            currentSpeed = Mathf.MoveTowards(currentSpeed, movementSpeed, acceleration * Time.deltaTime);
            
            animator.SetBool("Run", true);

            // Xoay theo hướng camera
            moveDirection = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z)) * moveDirection;
            var targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Giảm tốc dần
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            animator.SetBool("Run", false);
        }

        // Tính toán vận tốc di chuyển
        Vector3 velocity = moveDirection * currentSpeed;

        // Áp dụng trọng lực nếu không chạm đất
        if (!controller.isGrounded)
        {
            downwardVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        velocity.y = downwardVelocity;
        controller.Move(velocity * Time.deltaTime);
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

        // Tiến lên phía trước một chút trong lúc tấn công
        StartCoroutine(MoveForwardDuringAttack(movementSpeed / 3));
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

    private IEnumerator MoveForwardDuringAttack(float movementSpeed)
    {
        Vector3 forwardMovement = transform.forward;
        float moveTime = 0.1f; // Thời gian di chuyển
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            controller.Move(forwardMovement * movementSpeed * Time.deltaTime); // Di chuyển nhân vật
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
