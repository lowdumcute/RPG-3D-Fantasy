using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerStats : MonoBehaviour
{
    [SerializeField] public PlayerStats playerStats;
    public void AddSOStats()
    {
        GameManager.Instance.AddPlayerStats(playerStats);
    }
}
