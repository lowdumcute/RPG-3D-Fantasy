using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI_HUD_Manager : MonoBehaviour
{
    public static PlayerUI_HUD_Manager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);

        }
    }
    [SerializeField] private UIStat_Bar uIStat_Bar;// Start is called before the first frame update
    [SerializeField] private UIStat_Bar uIHealth_Bar;
    public void RefreshUI()
    {
        uIHealth_Bar.gameObject.SetActive(false);
        uIHealth_Bar.gameObject.SetActive(true);

        uIStat_Bar.gameObject.SetActive(false);
        uIStat_Bar.gameObject.SetActive(true);
    }
    public void SetNewHealthValue(float newValue)
    {
        uIHealth_Bar.SetStat(newValue);
    }
    public void SetNewMaxHealthValue(float MaxValue)
    {
        uIHealth_Bar.SetMaxStat(MaxValue);
    }
    public void SetNewStaminaValue(float newValue)
    {
        uIStat_Bar.SetStat(newValue);
    }
    public void SetNewMaxStaminaValue(float MaxStamina)
    {
        uIStat_Bar.SetMaxStat(MaxStamina);
    }
}
