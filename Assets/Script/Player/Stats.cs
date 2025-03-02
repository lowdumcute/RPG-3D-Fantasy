using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats 
{
    public event EventHandler OnStatsChanged;
    public static int STAT_MIN = 0;
    public static int STAT_MAX = 20;
    public enum Type { Attack, Defense, Speed, Mana, Health }    
    private SingleStat attackStat;
    private SingleStat defenseStat;
    private SingleStat speedStat;
    private SingleStat manaStat;
    private SingleStat healthStat;
    public Stats(int attackStatAmount, int defenseStatAmount, int speedStatAmount, int manaStatAmount, int healthStatAmount)
    {
        attackStat  = new SingleStat(attackStatAmount);
        defenseStat = new SingleStat(defenseStatAmount);
        speedStat   = new SingleStat(speedStatAmount);
        manaStat    = new SingleStat(manaStatAmount);
        healthStat  = new SingleStat(healthStatAmount);
    }
    private SingleStat GetSingleStat (Type statType)
    {
        switch (statType)
        {
            default: 
            case Type.Attack:   return attackStat;
            case Type.Defense:  return defenseStat;
            case Type.Speed:    return speedStat;
            case Type.Mana:     return manaStat;
            case Type.Health:   return healthStat;
        }
    }
    public void SetStatAmount(Type statType, int StatAmount)
    {
        GetSingleStat(statType).SetStatAmount(StatAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }
    public void IncreaseStatAmount(Type statType )
    {
        SetStatAmount(statType, GetStatAmount(statType) + 1);
    }
    public void DecreaseStatAmount(Type statType)
    {
        SetStatAmount(statType, GetStatAmount(statType) - 1);
    }
    public int GetStatAmount(Type statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }
    public float GetStatAmountNormalized(Type statType)
    {
         return GetSingleStat(statType).GetStatAmountNormalized();
    } 

    private class SingleStat
    {
        private int Stat;
        public SingleStat(int StatAmount)
        {
            SetStatAmount(StatAmount);
        }
        public void SetStatAmount(int StatAmount)
        {
            Stat = Math.Clamp(StatAmount, STAT_MIN, STAT_MAX);
        }
        public int GetStatAmount()
        {
            return Stat;
        }
        public float GetStatAmountNormalized()
        {
            return (float)Stat / STAT_MAX;
        } 
    }
}

