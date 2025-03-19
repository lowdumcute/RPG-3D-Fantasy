using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "GameItem/Equipment")]
public class EquipItemSO : ItemSO
{
    public GameObject WeaponPrefab;
    public ItemType itemType; //
}
