using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager playerManager;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;
    [Header("Setting")]
    [SerializeField] private Vector3 MoveDirection;
    [SerializeField] private Vector3 targetDirection;
    [SerializeField] private float WalkingSpeed  = 2f;
    [SerializeField] private float RunningSpeed =5f;
    [SerializeField] private float RotationSpeed = 5f;
    [Header("Dodge")]
    [SerializeField] private KeyCode Button;
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
        if (!playerManager.canmove) return;
        CheckGround();
        GetVerticalAndHorizontalInput();
        HandleRotation();
        MoveDirection = CameraController.Instance.transform.forward * verticalMovement;
        MoveDirection = MoveDirection + CameraController.Instance.transform.right * horizontalMovement;
        MoveDirection.Normalize();
        MoveDirection.y = 0f;
        
        if (PlayerInput.Instance.moveAmount >= 0.5 && PlayerInput.Instance.moveAmount < 1.5)
        {
            //Running
            playerManager.characterController.Move(MoveDirection * WalkingSpeed * Time.deltaTime);
            
        }
        else if (playerManager.canRun)
        {
            //running
            playerManager.characterController.Move(MoveDirection * RunningSpeed * Time.deltaTime);
            
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
    public void HandleRotation()
    {
        if (!playerManager.canRotate) return;
        targetDirection = Vector3.zero;
        targetDirection = CameraController.Instance.gameObject.transform.forward * verticalMovement;
        targetDirection = targetDirection + CameraController.Instance.gameObject.transform.right * horizontalMovement;
        targetDirection.y = 0f;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        Quaternion Newrotation = Quaternion.LookRotation(targetDirection);
        Quaternion TargerRotation = Quaternion.Slerp(transform.rotation, Newrotation, RotationSpeed * Time.deltaTime);
        transform.rotation = TargerRotation;
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
                playerManager.playerAnimtionManager.PlayTargetActionAnimation("Roll_Forward", true, true);
            }
            else
            {
                
                playerManager.playerAnimtionManager.PlayTargetActionAnimation("Slide_Forward", true, true);
            }
        }
        
    }
}
