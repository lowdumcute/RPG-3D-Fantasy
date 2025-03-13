using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    [Header("Movement Setting")]
    public static PlayerInput Instance;
    public PlayerManager PlayerManager; 

    [SerializeField] private Vector2 MovementInput;

    [HideInInspector]public float verticalInput;
    [HideInInspector] public float horizontalInput;
    public float moveAmount;

    

    private void Awake()
    {   
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            } 
    }

    private void Start()
    {
        PlayerManager = GetComponent<PlayerManager>();
    }
    // Update is called once per frame
    void Update()
    {
            
        MovementInput.x = Input.GetAxis("Horizontal");
        MovementInput.y = Input.GetAxis("Vertical");

        HanndleMovementInput();
    }
    private void HanndleMovementInput()
    {
        horizontalInput = MovementInput.x;
        verticalInput = MovementInput.y;
        
        //Trả về số luôn dương;
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Clamp01(Mathf.Abs(horizontalInput)));

        if(moveAmount<= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(PlayerManager.isRun)
        {
            moveAmount = 2;
        }
        //Horizontal = 0 vì đang không có lock on 
        
            PlayerManager.playerAnimtionManager.UpdateParamaterValue(0, moveAmount);
        
        //Nếu có Locked Target 
    }
    
}
