using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("StatusPlayer")]
    public PlayerManager playerManager;
    public PlayerStatus playerStatus;

    [Header("Tốc độ quay của nhân vật khi di chuyển")]
    public float TurnSmoothTime = 0.1f;
    private float TurnSmoothVelocity;

    [Header("Camera Xác định hướng đi")]
    public Transform cam;
    public Transform cameraPivot; // Thêm Camera Pivot để quản lý xoay camera

    [Header("Input Di chuyển của nhân vật")]
    public float HorizontalInput;
    public float VerticalInput;
    private CharacterController characterController;

    public Animator animator;
    private int _speedHash;

    [Header("Lock-On System")]
    public LockOnSystem lockOnSystem;
    public bool isLockedOn => lockOnSystem != null && lockOnSystem.currentTarget != null;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStatus = GetComponent<PlayerStatus>();
        characterController = GetComponent<CharacterController>();
        _speedHash = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(HorizontalInput, 0, VerticalInput).normalized;

        if (Direction.magnitude >= 0.1f)
        {
            if (playerManager.isPerformingAction)
                return;

            animator.SetBool("isMoving", true);
            
                MoveNormally(Direction);
            
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat(_speedHash, 0, 0.1f, Time.deltaTime);
        }

        HandleCamera();
    }

    private void MoveNormally(Vector3 Direction)
    {
        float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, Angle, 0f);

        Vector3 MoveDir = Quaternion.Euler(0, Angle, 0) * Vector3.forward;
        if (!characterController.isGrounded)
        {
            MoveDir += Vector3.down * 9.81f;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            characterController.Move(MoveDir * playerStatus.Speed * 1.5f * Time.deltaTime);
            animator.SetFloat(_speedHash, 2f, 0.1f, Time.deltaTime);
        }
        else
        {
            characterController.Move(MoveDir * playerStatus.Speed * Time.deltaTime);
            animator.SetFloat(_speedHash, Direction.magnitude, 0.1f, Time.deltaTime);
        }
        
    }

    private void HandleCamera()
    {
        if (isLockedOn && lockOnSystem.lockOnTargetPoint != null)
        {
            // Tính hướng từ cameraPivot đến mục tiêu
            Vector3 directionToTarget = lockOnSystem.lockOnTargetPoint.position - cameraPivot.position;

            // Thay đổi góc nhìn theo trục X để nhìn từ trên xuống
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            Vector3 eulerAngles = lookRotation.eulerAngles;
            eulerAngles.x = 45f; // Điều chỉnh góc nhìn từ trên xuống (thử nghiệm với giá trị khác nếu cần)

            // Cập nhật rotation với góc mới
            lookRotation = Quaternion.Euler(eulerAngles);
            cameraPivot.rotation = Quaternion.Slerp(cameraPivot.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

}
