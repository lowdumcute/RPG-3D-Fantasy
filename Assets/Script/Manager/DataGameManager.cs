using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[CreateAssetMenu(fileName = "DataGameManager", menuName = "Manager/DataGameManager")]
public class DataGameManager : ScriptableObject
{
    [SerializeField] public PlayerStats[] AllRoleStats;
    [SerializeField] public PlayerStats playerStatsUsing;
    [SerializeField] public int currentLevel;
    [SerializeField] private int exp;

}
[System.Serializable]
public class GameData
{
    public int level;
    public string Role;

}
