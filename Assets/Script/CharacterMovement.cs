using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    private float currentSpeed;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        animator.SetBool("Run", false);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isMoving = (moveX != 0 || moveZ != 0);
        animator.SetBool("Run", isMoving);

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Lấy hướng từ camera
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Đảm bảo hướng di chuyển không bị nghiêng theo góc camera
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Hướng di chuyển theo góc camera
        Vector3 move = camRight * moveX + camForward * moveZ;
        move.Normalize();

        // Xoay nhân vật theo hướng di chuyển
        if (isMoving)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }

        moveDirection.x = move.x * currentSpeed;
        moveDirection.z = move.z * currentSpeed;

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                moveDirection.y = jumpForce;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }
}
