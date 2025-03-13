using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character Effect/ Instant Effect/ Take Stamina Dame ")]
public class TakeStaminaDameEffect : InstantCharacterEffect
{
    public float DameAmount;
    public AudioClip SoundEffect;
    public override void ProcessEffect(PlayerManager playerManager)
    {
        CalculatedDamage(playerManager);
    }
    private void CalculatedDamage(PlayerManager playerManager)
    {
        //Tinh sanh Stamina voi cua player
        playerManager.isPerformingAction = true;
        playerManager.playerStatusManager.currentStamina -= DameAmount;
        PlayerUI_HUD_Manager.instance.SetNewStaminaValue(playerManager.playerStatusManager.currentStamina);
        playerManager.isPerformingAction = false;
        //cap nhat lai UI

        //Play sound Effect
    }
}
