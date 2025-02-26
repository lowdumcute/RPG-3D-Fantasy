using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private KeyCode keyCodeInventory = KeyCode.Escape;
    [SerializeField] private List<ItemSlot> itemsSlot = new List<ItemSlot>();
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private Transform slotContainer; // Chỗ chứa các SlotPrefab

    private bool isActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        isActive = false;
        InventoryMenu.SetActive(isActive);
        GenerateInventorySlots(); // Tạo các slot theo số lượng itemsSlot
        DeSelectedAllItemSlot();
    }

    private void Update()
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

    private void GenerateInventorySlots()
    {
        // Xóa slot cũ nếu có
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        itemsSlot.Clear(); // Xóa danh sách cũ

        // Spawn các slot mới
        for (int i = 0; i < itemsSlot.Capacity; i++)
        {
            GameObject newSlot = Instantiate(SlotPrefab, slotContainer);
            ItemSlot slotComponent = newSlot.GetComponent<ItemSlot>();
            if (slotComponent != null)
            {
                itemsSlot.Add(slotComponent);
            }
        }
    }

    public void AddItem(ItemSO item, int quantity)
    {
        var Item = itemsSlot.Find(i => i.ID == item.ID);
        if (Item != null)
        {
            Item.ItemQuantity += quantity;
        }
        else
        {
            foreach (ItemSlot slot in itemsSlot)
            {
                if (!slot.isHaveItem)
                {
                    slot.AddItem(item, quantity);
                    return;
                }
            }
        }
    }

    public void DeSelectedAllItemSlot()
    {
        foreach (ItemSlot slot in itemsSlot)
        {
            slot.SelectedPanel.SetActive(false);
            slot.isSelected = false;
        }
    }

    public void RefreshInventory()
    {
        foreach (ItemSlot slot in itemsSlot)
        {
            slot.QuantityText.text = slot.ItemQuantity.ToString();
        }
    }
}
