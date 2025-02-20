using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float jumpForce = 10f;
    private bool isRolling = false; // Biến kiểm tra trạng thái roll
    private float rollSpeed = 10f;  // Tốc độ của roll
    private float rollDuration = 0.5f; // Thời gian kéo dài roll

    [Header("Attack")]
    private int comboStep = 0;  // Để theo dõi bước combo
    private float attackCooldown = 0.01f;
    private float comboTimeLimit = 2f;  // Thời gian cho phép nhấn chuột liên tục để combo
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

        Vector3 velocity = new Vector3(horizontal, 0, vertical).normalized * movementSpeed;
        velocity = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z)) * velocity;

        // Áp dụng trọng lực nếu không chạm đất
        if (!controller.isGrounded)
        {
            downwardVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        velocity.y = downwardVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            animator.SetBool("Run", true);
            var targetRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            downwardVelocity = -2f; // Đặt giá trị thấp để tránh bị "dính" mặt đất

            if (Input.GetButtonDown("Jump"))
            {
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

        // Xác định hướng nhìn của camera nhưng bỏ đi trục Y
        Vector3 cameraForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = targetRotation;

        // Tiến lên phía trước một chút trong lúc tấn công
        StartCoroutine(MoveForwardDuringAttack(movementSpeed/3));
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
            controller.Move(rollDirection * rollSpeed * Time.deltaTime); // Di chuyển nhân vật theo roll
            rollTime += Time.deltaTime;
            yield return null;
        }

        isRolling = false;  // Kết thúc roll
    }
}
