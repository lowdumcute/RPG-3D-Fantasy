using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character Effect/ Instant Effect/ Take Dame ")]
public class TakingDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public PlayerManager player;
    [Header("Damage")]
    public float PhysicalDamage = 0;
    public float MagicDamage = 0;
    public float LightingDamage = 0;
    public float FireDamage = 0;
    public float HolyDamage = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false; // Nếu poise = 0 thì bị stun sau đấy animation Stun

    [Header("Animation")]
    // Damage Animation
    // PlaydamageAnimation 
    [Header("Final Damage")]
    private float FinalDamage = 0f;
    [Header("SFX")]
    public bool PlaySFX = true;
    public AudioClip ElementalDamageSoundSFX;
    [Header("Recive Direction Damage")]
    public float angleDamage;
    public Vector3 contractPoint;
    public override void ProcessEffect(PlayerManager playerManager)
    {
        if (playerManager.playerStatusManager.isDead)
            return;

        CalculatedDamage(playerManager);
        base.ProcessEffect(playerManager);
        
        
    }
    private void CalculatedDamage(PlayerManager playerManager)
    {
        if(playerManager != null)
        {
            FinalDamage = (PhysicalDamage + MagicDamage + FireDamage + LightingDamage + HolyDamage);
        }
        if(FinalDamage <= 0)
        {
            FinalDamage = 1;
        }
        Debug.Log($"Damage {FinalDamage}");
        playerManager.playerStatusManager.currentHealth -= FinalDamage;
        PlayerUI_HUD_Manager.instance.SetNewHealthValue(playerManager.playerStatusManager.currentHealth);
    }
}
