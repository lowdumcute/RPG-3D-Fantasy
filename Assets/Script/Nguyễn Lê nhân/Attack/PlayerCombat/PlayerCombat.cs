using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player manager")]
    public PlayerManager playerManager;

    [Header("Các Biến Combo")]
    public List<AttackSO> combo;
    public float lastClickedTime;
    public float LastComboEnd;
    int ComboCounter;
    [Header("Vũ khí")]
    public Weapon weapon;
    [Header("Animation")]
    Animator animator;
    bool isAttacking;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        weapon.EnableTriggerBox();
        
    }
    public void EndAttack()
    {
        
        weapon.DisableTriggerBox();
    }
}

