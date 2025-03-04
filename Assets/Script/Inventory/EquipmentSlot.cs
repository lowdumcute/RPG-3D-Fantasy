using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Slot Data")] // Dữ Liệu của Item
    [SerializeField] public EquipItemSO itemSO;
    [SerializeField] public int ID;
    [SerializeField] private string ItemName;
    [SerializeField] private ItemType itemType;

    [SerializeField] private Sprite IconItemSprite;
    [SerializeField] private string ItemDecription;
    public bool isHaveItem;

    [Header("Item Slot")]// Dữ liệu của Item nhưng ở ngoài UI
    public Image ItemImage;
    public GameObject SelectedPanel;
    public bool isSelected;
    //Equipment Slot
    //Nơi chứa Trang bị 
    [SerializeField]
    private EquipqedSlot Head, Body, Glove, Boots, MainHand, OffHand, Relic, Relic2;


    [Header("Invetory Manager")]    //Inventory của người chơi
    [SerializeField] private InventoryManager inventoryManager;
    public PlayerHealthManager PlayerHealth;// máu của người chơi

    private void Start()
    {
        RefreshInfo();
        inventoryManager = GameObject.Find("Inventory").gameObject.GetComponent<InventoryManager>();
        
    }

    //Hàm gọi khi mới Add 1 vật phẩm mới vào Inventory
    public void AddItem(EquipItemSO itemSO, ItemType itemType)
    {
        this.itemSO = itemSO;
        this.ItemName = itemSO.ItemName;
        this.ID = itemSO.ID;
        this.itemType = itemType;
        this.IconItemSprite = itemSO.Icon;
        this.ItemDecription = itemSO.Decription;
        ItemImage.enabled = true; ;
        ItemImage.sprite = IconItemSprite;
        isHaveItem = true;
        RefreshInfo();
    }
    //Hàm sử dụng Item
    // Hàm reset thông tin của ItemSlot khi ItemQuantity <= 0
    public void ResetItemSlot()
    {
        itemSO = null;
        ID = 0;
        ItemName = "";
        IconItemSprite = null;
        ItemDecription = "";
        isHaveItem = false;
        isSelected = false;

        
        ItemImage.sprite = null;
        SelectedPanel.SetActive(false);

        

        // tắt hình ảnh mô tả và nút sử dụng khi vật phẩm hết
        
        ItemImage.enabled = false;
        
    }


    // Cập nhật lại UsingItem()
    


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)// kiểm tra điều kiện nhấn chuột trái
        {
            if (!isHaveItem)
            {
                return;
            }
            else
            {
                OnLeftClick();
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)// kiểm tra điều kiện nhấn chuột phải
        {
            OnRightClick();
        }
    }
    void OnLeftClick()//Nếu nhấn chuột trái reset lại tất cả UI panel đã chọn sang cái đang chọn
    {
        inventoryManager.DeSelectedAllItemSlot();

        isSelected = true;
        SelectedPanel.SetActive(true);

        // Đảm bảo chỉ thêm sự kiện cho nút khi chọn item
        
        
        RefreshInfo();

    }
    void OnRightClick()//Nhấn chuột phải vào slot để sử dụng Item
    {
        EquipGear();
    }
    void EquipGear()
    {
        if (itemType == ItemType.Head)
            Head.Equipmentgear(itemSO,IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.Body)
            Body.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.Glove)
            Glove.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.Boots)
            Boots.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.weapon)
            MainHand.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.Collectible)
            OffHand.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.Collectible)
            Relic.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        if (itemType == ItemType.Collectible)
            Relic2.Equipmentgear(itemSO, IconItemSprite, ItemName, ItemDecription);
        ResetItemSlot();
    }
    public void RefreshInfo()//sau khi nhấn thì làm mới lại thông tin vật phẩm 
    {
        if (isHaveItem)
        {

            ItemImage.enabled = true;
        }
        else
        {
            ItemImage.enabled = false;
            
        }
        if (isSelected)
        {
            SelectedPanel.SetActive(true);

        }
        else
        {
            SelectedPanel.SetActive(false);

        }    
        ItemImage.sprite = IconItemSprite;
    }

}
