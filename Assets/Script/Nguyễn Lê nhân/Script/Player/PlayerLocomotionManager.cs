using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager playerManager;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public float turnTime = 0.1f;
    [HideInInspector] public float turnSmoothVelocity;
    [Header("Setting")]
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 MoveDirection;
    [SerializeField] private Vector3 targetDirection;
    [SerializeField] private float WalkingSpeed  = 2f;
    [SerializeField] private float RunningSpeed =5f;
    [SerializeField] private float RotationSpeed = 5f;
    [SerializeField] private float StaminaCost = 5f;
    
    [Header("Dodge")]    
    private Vector3 RollDirection;
    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        
    }
    private void GetVerticalAndHorizontalInput()
    {
        verticalMovement = PlayerInput.Instance.verticalInput;
        horizontalMovement = PlayerInput.Instance.horizontalInput;
        moveAmount = PlayerInput.Instance.moveAmount;
    }
    public void HandheldAllMovement()
    { 
        //Ground Movement
        HandleGroundMovement();
        //AirMovement
        //
    }
    public void HandleGroundMovement()
    {
        if (!playerManager.canmove|| !playerManager.canRotate) return;
        CheckGround();
        GetVerticalAndHorizontalInput();

        Vector3 Direction = new Vector3(horizontalMovement, 0f, verticalMovement).normalized;

        if (Direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;



            // Nếu stamina hết -> ép nhân vật đi bộ
            if (playerManager.isRun && playerManager.playerStatusManager.currentStamina <= 0 )
            {
                playerManager.isRun = false; // Tắt trạng thái chạy
                playerManager.playerAnimtionManager.UpdateParamaterValue(0, 0.5f);
            }

            if (playerManager.isRun)
            {
                playerManager.characterController.Move(MoveDirection * RunningSpeed * Time.deltaTime);
                float stamina = playerManager.playerStatusManager.currentStamina -= StaminaCost * Time.deltaTime;
                PlayerUI_HUD_Manager.instance.SetNewStaminaValue(stamina);
                playerManager.playerStatusManager.StaminaRegeTimer = 0;
            }
            else
            {
                playerManager.characterController.Move(MoveDirection * WalkingSpeed * Time.deltaTime);
            }
        }
        
    }
    void CheckGround()
    {
        if (!playerManager.characterController.isGrounded)
        {
            Vector3 downwardForce = new Vector3(0, -9.81f * Time.deltaTime, 0);
            playerManager.characterController.Move(downwardForce);
        }
    }
    
    public void AttemtoDodge()
    {
        if (playerManager.isPerformingAction == true) return;
        if(PlayerInput.Instance.moveAmount > 0)
        {
            RollDirection = CameraController.Instance.gameObject.transform.forward * PlayerInput.Instance.verticalInput;
            RollDirection += CameraController.Instance.gameObject.transform.right * PlayerInput.Instance.horizontalInput;
            RollDirection.y = 0f;
            Quaternion playerRotation = Quaternion.LookRotation(RollDirection);
            playerManager.transform.rotation = playerRotation;
            if(!playerManager.canSlide)
            {
               if(playerManager.playerStatusManager.currentStamina > 5)
                {
                    playerManager.playerAnimtionManager.PlayTargetActionAnimation("Roll_Forward", true, true);
                    float stamina = playerManager.playerStatusManager.currentStamina -= 5;
                    PlayerUI_HUD_Manager.instance.SetNewStaminaValue(stamina);
                }
            }
            else
            {
                
                playerManager.playerAnimtionManager.PlayTargetActionAnimation("Slide_Forward", true, true);
            }
        }
        
    }
}
