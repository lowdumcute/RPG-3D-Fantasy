using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : CharactersStatusManager
{
    [Header("Chỉ số")] 
    //Máu
    public float currentHealth;
    public float maxHealth;
    //thể lực
    public float currentStamina;
    public float maxStamina;
    //Mana
    public int currentMana;
    public int maxmana;
    [Header("Hồi phục Stamina")]
    public float StaminaRegeAmount = 5;
    public float StaminaRegeTimer = 0;
    public float StaminaRegeTickTimer = 0;
    public float StaminaDelay = 2f;
    private void Awake()
    {
        maxStamina = CalculatingStaminaBaseOnlevel(Endurence);
        maxHealth = CalculatingHealthOnlevel(Vigor);

        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
    public virtual void RegenerateStamina()
    {
        StaminaRegeTimer += Time.deltaTime;
        if(StaminaRegeTimer > StaminaDelay)
        {
            if(currentStamina < maxStamina)
            {
                StaminaRegeTickTimer += Time.deltaTime;
                if(StaminaRegeTickTimer >= 0.1)
                {
                    StaminaRegeTickTimer = 0;
                    float stamina =currentStamina += StaminaRegeAmount * Time.deltaTime;
                    PlayerUI_HUD_Manager.instance.SetNewStaminaValue(stamina);
                }
            }
        }
    }
    public int CalculatingStaminaBaseOnlevel(int levelEndurence)
    {
        float Stamina = 0;

        Stamina = levelEndurence * 10f;
        return Mathf.RoundToInt(Stamina);
    }
    public int CalculatingHealthOnlevel(int levelVigor)
    {
        float Health = 0;
        Health = levelVigor * 30f;
        return Mathf.RoundToInt(Health);
    }
}
