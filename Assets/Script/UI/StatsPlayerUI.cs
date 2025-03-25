using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsPlayerUI : MonoBehaviour
{
    public static StatsPlayerUI Instance;
    [SerializeField] private DataGameManager dataGameManager;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ManaText;
    [SerializeField] private TMP_Text AttackText;
    [SerializeField] private TMP_Text DefenseText;
    [SerializeField] private TMP_Text SpeedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        HealthText.text =   "<color=#FF4C4C>Health  : " + dataGameManager.playerStatsUsing.maxHealth + "</color>";
        ManaText.text =     "<color=#4C8CFF>Mana   : " + dataGameManager.playerStatsUsing.maxMana + "</color>";
        AttackText.text =   "<color=#FFD700>Attack  : " + dataGameManager.playerStatsUsing.maxAttack + "</color>";
        DefenseText.text =  "<color=#4CFF4C>Defense : " + dataGameManager.playerStatsUsing.maxAttack + "</color>";
        SpeedText.text =    "<color=#FF8C4C>Speed   : " + dataGameManager.playerStatsUsing.maxSpeed + "</color>";
    }
}
