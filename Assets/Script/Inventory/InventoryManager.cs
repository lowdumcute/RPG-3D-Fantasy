using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private KeyCode keyCodeInventory = KeyCode.Escape;
    [SerializeField] private List<ItemSlot> itemsSlot;
    
    bool isActive;
    void Start()
    {
        isActive = false;
        InventoryMenu.SetActive(isActive);
        DeSelectedAllItemSlot();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCodeInventory))
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
    }
    
    public void AddItem(ItemSO item, int quantity)// hàm add vào Inventory
    {

        var Item = itemsSlot.Find(i => i.ID == item.ID); //kiểm tra có ID trong inventory hay không 
        if (Item != null)
        {
            //nếu có thì cộng số lượng 
            Item.ItemQuantity += quantity;
        }
        else// nếu không thì add mới vào 
        {
            foreach (ItemSlot slot in itemsSlot)
            {
                if (slot.isHaveItem== false)
                {
                    slot.AddItem(item, quantity);
                    return;
                }
            }
        }

    }
    public void DeSelectedAllItemSlot()
    {
        foreach (ItemSlot slot in itemsSlot)//Tắt slotđã  chọn trước đó khi chon 1 panel khác 
        {
            slot.SelectedPanel.SetActive(false);
            slot.isSelected = false;
        }
    }
    public void RefreshInventory()// Làm mới thông tin Inventory
    {
        foreach (ItemSlot slot in itemsSlot)
        {
            slot.QuantityText.text = slot.ItemQuantity.ToString();

        }

    }
}
