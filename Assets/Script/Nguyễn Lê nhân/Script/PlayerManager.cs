using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public PlayerLocomotionManager locomotionManager;
    public CharacterAnimationManger playerAnimtionManager;
    protected override void Awake()
    {
        base.Awake();
        // DO more Stuff, for player

    }
    private void Start()
    {
        locomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimtionManager = GetComponent<CharacterAnimationManger>();
    }
    
    protected override void Update()
    {
        base.Update();
        locomotionManager.HandheldAllMovement();
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Dodge");
            locomotionManager.AttemtoDodge();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Run");
            canRun = true;
            canSlide = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("quit Run");
            canRun = false;
            canSlide = false;
        }
    }

}

