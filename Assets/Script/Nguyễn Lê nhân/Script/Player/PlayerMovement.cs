using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("StatusPlayer")]
    public PlayerManager playerManager;
    public PlayerStatus playerStatus;

    [Header("Tốc độ quay của nhân vật khi di chuyển")]
    public float TurnSmoothTime = 0.1f;
    public float TurnSmoothVelocity;
    [Header("Camera Xác định hướng đi")]
    public Transform cam;
    [Header("Input Di chuyển của nhân vật")]
    public float HorizontalInput;
    public float VerticalInput;
    private CharacterController characterController;

    public Animator animator;
    public int _speadHash;
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStatus = GetComponent<PlayerStatus>();
        characterController = GetComponent<CharacterController>();
        _speadHash = Animator.StringToHash("VelocityZ");
    }

    // Update is called once per frame

    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        var Direction = new Vector3(HorizontalInput, 0, VerticalInput).normalized;

        if (Direction.magnitude >= 0.1f)
        {
            if (playerManager.isPerformingAction)
                return;
            animator.SetBool("isMoving", true);
            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, Angle, 0f);

            Vector3 MoveDir = Quaternion.Euler(0, Angle, 0) * Vector3.forward;
            if (!characterController.isGrounded)
            {
                MoveDir += Vector3.down * 9.81f;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move(MoveDir * (playerStatus.Speed * 1.5f) * Time.deltaTime);
                animator.SetFloat(_speadHash, 2f, 0.1f, Time.deltaTime);
                

            }
            else
            {
                
                characterController.Move(MoveDir * playerStatus.Speed * Time.deltaTime);
                animator.SetFloat(_speadHash, Direction.magnitude, 0.1f, Time.deltaTime);
            }
            

        }
        else
        {
            
            animator.SetBool("isMoving", false);
            animator.SetFloat(_speadHash, Direction.magnitude, 0.1f, Time.deltaTime);
        }
    }
}
