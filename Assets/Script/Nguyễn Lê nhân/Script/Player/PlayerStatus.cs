using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharactersStatusManager
{
    [Header("Chỉ Số máu")]
    public float MaxHealth;
    public float CurrentHealth;
    [Header("Chỉ số Mana")]
    public float MaxMana;
    public float CurrentMana;
    [Header("Chỉ số thể lực")]
    public float MaxStamina;
    public float CurrentStamina;
    [Header("Chỉ số phòng thủ")]
    public float Defense;
    [Header("Chỉ số phòng tốc độ")]
    public float Speed;
}
