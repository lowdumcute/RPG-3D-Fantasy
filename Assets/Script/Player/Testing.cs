using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using System;

public class Testing : MonoBehaviour
{
    [SerializeField]private UIStatsRadarChart uiStatsRadarChart;
    [SerializeField] private PlayerStats playerStats;
    private void Start()
    {
        Stats stats = new Stats(playerStats.DAttack, playerStats.DDefense, playerStats.DSpeed, playerStats.DMana, playerStats.DHealth);

        uiStatsRadarChart.SetStats(stats);
        CMDebug.ButtonUI(new Vector2(200, +20), "ATK++", () => stats.IncreaseStatAmount(Stats.Type.Attack));
        CMDebug.ButtonUI(new Vector2(200, -20), "ATK--", () => stats.DecreaseStatAmount(Stats.Type.Attack));
        CMDebug.ButtonUI(new Vector2(300, +20), "DEF++", () => stats.IncreaseStatAmount(Stats.Type.Defense));
        CMDebug.ButtonUI(new Vector2(300, -20), "DEF--", () => stats.DecreaseStatAmount(Stats.Type.Defense));
        CMDebug.ButtonUI(new Vector2(400, +20), "SPE++", () => stats.IncreaseStatAmount(Stats.Type.Speed));
        CMDebug.ButtonUI(new Vector2(400, -20), "SPE--", () => stats.DecreaseStatAmount(Stats.Type.Speed));
        CMDebug.ButtonUI(new Vector2(500, +20), "MNA++", () => stats.IncreaseStatAmount(Stats.Type.Mana));
        CMDebug.ButtonUI(new Vector2(500, -20), "MNA--", () => stats.DecreaseStatAmount(Stats.Type.Mana));
        CMDebug.ButtonUI(new Vector2(600, +20), "HLT++", () => stats.IncreaseStatAmount(Stats.Type.Health));
        CMDebug.ButtonUI(new Vector2(600, -20), "HLT--", () => stats.DecreaseStatAmount(Stats.Type.Health));

    }
}
