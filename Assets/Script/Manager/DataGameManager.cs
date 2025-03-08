
using UnityEngine;
[CreateAssetMenu(fileName = "DataGameManager", menuName = "Manager/DataGameManager")]
public class DataGameManager : ScriptableObject
{
    [SerializeField] public PlayerStats[] AllRoleStats;
    [SerializeField] public PlayerStats playerStatsUsing;
    [SerializeField] public int currentLevel;
    [SerializeField] private int exp;
    [SerializeField] public Vector3 Position;

}
[System.Serializable]
public class GameData
{
    public int level;
    public string Role;
    public Vector3 position;
    public string SceneSave;
}
