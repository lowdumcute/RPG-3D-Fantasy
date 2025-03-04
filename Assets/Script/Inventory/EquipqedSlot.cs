using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipqedSlot : MonoBehaviour
{

    //UI của Slot Equip
    [SerializeField] private Image SlotImage;
    [SerializeField]
    private TextMeshProUGUI slotName;

    //Slot Data
    [SerializeField]
    private ItemType itemType = new ItemType();
    [SerializeField] private EquipItemSO equipItem;
    private Sprite itemSprite;
    private string itemName;
    private string itemDecription;
    //Biến khác 
    bool inUse;
    //Biến chứa các Game Prefab và instance
    GameObject currentWeapon;
    public GameObject HandlerWeapon;
    
    public void Equipmentgear(EquipItemSO EquipItem, Sprite itemSprite, string name, string itemDecription)
    {
        //Update Image
        this.itemSprite = itemSprite;
        SlotImage.sprite = this.itemSprite;
        slotName.enabled = false;
        //Update Data
        this.equipItem = EquipItem;
        this.itemName = name;
        this.itemDecription = itemDecription;
        if(HandlerWeapon != null)
        {
            currentWeapon = Instantiate(equipItem.WeaponPrefab);
            currentWeapon.transform.SetParent(HandlerWeapon.transform, false);
        }
    }
    public void UnEquipmentgear()
    {
        
    }
}
