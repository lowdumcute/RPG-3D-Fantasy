using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public  PlayerLocomotionManager locomotionManager;
    public  CharacterAnimationManger playerAnimtionManager;
    public PlayerInventoryManager PlayerinventoryManager;
    [Header("Button")]
    [SerializeField] private KeyCode Dodge;
    [SerializeField] private KeyCode Run;
  
    protected override void Awake()
    {
        base.Awake();
        // DO more Stuff, for player
        playerStatusManager.CalculatingHealthOnlevel(playerStatusManager.Vigor);
        playerStatusManager.CalculatingStaminaBaseOnlevel(playerStatusManager.Endurence);



    }
    private void Start()
    {
        locomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimtionManager = GetComponent<CharacterAnimationManger>();
        playerStatusManager = GetComponent<PlayerStatusManager>();
        PlayerinventoryManager = GetComponent<PlayerInventoryManager>();

        float stamina = playerStatusManager.maxStamina;
        PlayerUI_HUD_Manager.instance.SetNewMaxStaminaValue(stamina);

        float Health = playerStatusManager.maxHealth;
        PlayerUI_HUD_Manager.instance.SetNewMaxHealthValue(Health);

    }
    
    protected override void Update()
    {
        base.Update();
        locomotionManager.HandheldAllMovement();
        CheckRegeStamina();

        if (Input.GetKeyDown(Dodge))
        {
            Debug.Log("Dodge");
            locomotionManager.AttemtoDodge();
        }
        if (Input.GetKey(Run))
        {
            Debug.Log("Run");
            isRun = true;
            canSlide = true;
        }
        if (Input.GetKeyUp(Run))
        {
            Debug.Log("quit Run");
            isRun = false;
            canSlide = false;
        }
    }
    private void CheckRegeStamina()
    {
        if (isPerformingAction) return;
        if (isRun) return;
        playerStatusManager.RegenerateStamina();
        float stamina = playerStatusManager.currentStamina;
        PlayerUI_HUD_Manager.instance.SetNewStaminaValue(stamina);
    }
}

