using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterController characterController;
    public PlayerStatusManager playerStatusManager;
    public CharacterEffectManager characterEffectManager;
    public Animator animator;

    [Header("Flag")]
    public bool isPerformingAction = false;
    public bool canmove = true;
    public bool canRotate = true;
    public bool isRun = true;
    public bool canSlide = false;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        characterEffectManager = GetComponent<CharacterEffectManager>();
        playerStatusManager = GetComponent<PlayerStatusManager>();
        animator = GetComponent<Animator>();
    }
    protected virtual void Update()
    {

    }
}
