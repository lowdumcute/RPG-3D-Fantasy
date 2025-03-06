using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponitem : Item
{
    //Animator Controller Chỉnh lại animation dựa theo vũ khí đang sử dụng
    [Header("Weapon Model")]
    public GameObject WeaponPrefab;
    [Header("Weapon Requiments Status")]
    public int StregthREQ = 0;
    public int DexREQ = 0;
    public int IntelREQ = 0;
    public int FaitREQ = 0;
    [Header("Weapon Base Dame")]
    public int PhysicalDamage = 0;
    public int MagicDamage = 0;
    public int FireDamage = 0;
    public int HolycDamage = 0;
    public int LightingDamage = 0;
    //Weapon Guar Power
    [Header("Weapon Poise Dame")]
    public float poiseDame = 10f;
    //Weapon Modifer
    //Light Attack MOdifer
    //Heavy Attack Modifer
    //Critical DameModifier
    [Header("Stamina Cost")]
    public int BaseStaminaCost = 20;
    //Running Attack Cost
    //Light Attack Cost
    //HeavyAttack Cost

    //Weapon Deflection

    //Weapon base Action
    //Ash of War
    //Blocking sound
}
