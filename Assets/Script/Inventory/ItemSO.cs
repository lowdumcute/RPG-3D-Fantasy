using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Item", menuName = "GameItem/Item")]
public class ItemSO : ScriptableObject
{
    public ItemType Type;   //Loại Item
    public StatusChange Status;
    public float NumberOfChange;
    public string ItemName; // tên Item
    public int ID;          //Id của Item   
    public Sprite Icon;     // Icon của item
    public string Decription;//thông tin của item
}
public enum ItemType { Consume, Weapon, }
public enum StatusChange { Healt,Mana,Stamina}
