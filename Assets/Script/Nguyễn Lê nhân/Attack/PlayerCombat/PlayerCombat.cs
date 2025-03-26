using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;
    [Header("Player manager")]
    public PlayerManager playerManager;

    [Header("Các Biến Combo")]
    public List<AttackSO> combo;
    public float lastClickedTime;
    public float LastComboEnd;
    int ComboCounter;
    [Header("Vũ khí")]
    public Weapon WeaponinHand;
    [Header("Animation")]
    Animator animator;
    bool isAttacking;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
    }
    public void SetWeaponCombo(Weapon WeaponPrefab)
    {
        combo.Clear();
        WeaponinHand = WeaponPrefab;
        for (int i = 0;i < WeaponinHand.combo.Count; i++ )
        {
            combo.Add(WeaponinHand.combo[i]);
        }
    }
    public void RemoveCombo()
    {
        for (int i = 0; i < combo.Count; i++)
        {
            combo.Clear();
        }
        WeaponinHand = null;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (  combo.Count <= 0 ) return;
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Attack");
            
            Attack();
        }
        ExitAttack();
    }
    void Attack()
    {
        if (playerManager.isPerformingAction) return;
        if (isAttacking) return; // Không cho phép spam tấn công

        if (Time.time - LastComboEnd > 0.5f && ComboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");
            if(Time.time - lastClickedTime >=0.2f)
            {
                playerManager.isPerformingAction = true;
                animator.runtimeAnimatorController = combo[ComboCounter].animatorOverrideController;
                animator.Play("Attack", 0, 0.1f);
                
                ComboCounter++;
                
                lastClickedTime = Time.time;
                if(ComboCounter >= combo.Count)
                {
                    ComboCounter = 0;
                    
                }
            }
        }

        
    }
    void ExitAttack()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&  animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            
            Invoke("EndCombo",1);
            
        }
    }
    void EndCombo()
    {
        ComboCounter = 0;
        LastComboEnd = Time.time;
    }

   
    public void BeginAttack()
    {
        WeaponinHand.EnableTriggerBox();
        
    }
    public void EndAttack()
    {

        WeaponinHand.DisableTriggerBox();
    }
}

