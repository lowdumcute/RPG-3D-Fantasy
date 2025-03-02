using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } // Singleton

    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private GameObject EquipmentMenu;
    [SerializeField] private KeyCode keyCodeInventory = KeyCode.Escape;
    [SerializeField] private KeyCode keyCodeEquipment = KeyCode.Escape;
    [SerializeField] private List<ItemSlot> itemsConsumeSlot;
    [SerializeField] private List<ItemSlot> itemsWeaponSlot;
    private bool isActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Đảm bảo chỉ có 1 instance tồn tại
            return;
        }
    }

    private void Start()
    {
        isActive = false;
        InventoryMenu.SetActive(isActive);
        DeSelectedAllItemSlot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCodeInventory))
        {
            ConsumeInventory();
        }
        else if(Input.GetKeyDown(keyCodeEquipment))
        {
            EquipmentInventory();
        }
        
    }
    void ConsumeInventory()
    {
        isActive = !isActive;
        InventoryMenu.SetActive(isActive);
        RefreshInventory();

        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameraController.isPaused = true; // Tắt camera
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CameraController.isPaused = false; // Bật lại camera
        }
    }
    void EquipmentInventory()
    {
      
        RefreshInventory();

        if (EquipmentMenu.activeSelf)
        {
            EquipmentMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameraController.isPaused = true; // Tắt camera
        }
        else
        {
            EquipmentMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CameraController.isPaused = false; // Bật lại camera
        }
    }

    public void AddItem(ItemSO item, int quantity,ItemType itemtype)
    {
        if(itemtype == ItemType.Consume)
        {
            var Item = itemsConsumeSlot.Find(i => i.ID == item.ID);
            if (Item != null)
            {
                Item.ItemQuantity += quantity;
            }
            else
            {
                foreach (ItemSlot slot in itemsConsumeSlot)
                {
                    if (!slot.isHaveItem)
                    {
                        slot.AddItem(item, quantity,itemtype);
                        return;
                    }
                }
            }
        }
        else if( itemtype == ItemType.weapon|| itemtype == ItemType.Head ||
                 itemtype == ItemType.Body  || itemtype == ItemType.Glove|| 
                 itemtype == ItemType.Boots || itemtype == ItemType.Collectible)
        {
            var Item = itemsWeaponSlot.Find(i => i.ID == item.ID);
            if (Item != null)
            {
                Item.ItemQuantity += quantity;
            }
            else
            {
                foreach (ItemSlot slot in itemsConsumeSlot)
                {
                    if (!slot.isHaveItem)
                    {
                        slot.AddItem(item, quantity, itemtype);
                        return;
                    }
                }
            }
        }
    }

    public void DeSelectedAllItemSlot()
    {
        foreach (ItemSlot slot in itemsConsumeSlot)
        {
            slot.SelectedPanel.SetActive(false);
            slot.isSelected = false;
        }
    }

    public void RefreshInventory()
    {
        foreach (ItemSlot slot in itemsConsumeSlot)
        {
            slot.QuantityText.text = slot.ItemQuantity.ToString();
        }
    }
}
