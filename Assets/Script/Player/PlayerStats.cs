using System;
using UnityEngine;
public enum Type {Warrior, Archer, Assasin, Mage}

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] public String NameRole;
    [Header("Default Stats")]
    [SerializeField] public int DAttack = 10;
    [SerializeField] public int DDefense = 15;
    [SerializeField] public int DSpeed = 5;
    [SerializeField] public int DMana = 16;
    [SerializeField] public int DHealth = 18; 
    [Header("Base Stats")]
    public float maxHealth ;
    public int maxMana ;
    public int maxAttack ;
    public int maxSpeed ;
    public int maxDefend ;

    // Lưu giá trị hiện tại (thay đổi trong game)
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMana;

    // Hàm khởi tạo lại giá trị khi bắt đầu game
    public void Initialize()
    {
        UpdateStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
        
    }
    public void UpdateStats()
    {
        maxHealth   = DHealth * 3;
        maxMana     = DMana * 3;
        maxAttack   = DAttack * 3;
        maxSpeed    = DSpeed * 3;
        maxDefend   = DDefense * 3;
    }
}
