using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemAfterDrop : MonoBehaviour, IDropHandler
{
    public EquipmentSlot equipSlot; // Slot hiện tại mà item sẽ được thả vào
    public Image imageitem;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called");

        GameObject droppedItem = eventData.pointerDrag;
        if (droppedItem == null)
        {
            Debug.Log("Dropped item is null");
            return;
        }

        // Lấy EquipmentSlot từ item bị kéo thả
        EquipmentSlot oldSlot = droppedItem.GetComponentInChildren<EquipmentSlot>();
        if (oldSlot == null)
        {
            Debug.LogError("Dropped item không nằm trong EquipmentSlot!");
            return;
        }

        Debug.Log($"Item dropped from slot: {oldSlot.name} -> {equipSlot.name}");

        // Nếu slot đích có item, hoán đổi
        if (equipSlot.isHaveItem)
        {
            SwapItems(oldSlot, equipSlot);
        }
        else
        {
            // Nếu slot đích trống, chỉ cần đặt item vào
            equipSlot.AddItem(oldSlot.itemSO, oldSlot.itemType);
            
        }

        // Cập nhật hình ảnh UI
        UpdateUI(equipSlot);
        UpdateUI(oldSlot);
    }

    private void SwapItems(EquipmentSlot slotA, EquipmentSlot slotB)
    {
        // Nếu cùng loại trang bị, mới cho swap
        if (slotA.itemType != slotB.itemType)
        {
            Debug.Log("Không thể hoán đổi do khác loại trang bị!");
            return;
        }

        // Lưu dữ liệu của slotB (slot đích)
        EquipItemSO tempItemSO = slotB.itemSO;
        ItemType tempItemType = slotB.itemType;

        // Hoán đổi dữ liệu
        slotB.AddItem(slotA.itemSO, slotA.itemType);
        slotA.AddItem(tempItemSO, tempItemType);

        Debug.Log($"Swapped items: {slotA.ItemName} <-> {slotB.ItemName}");
    }

    private void UpdateUI(EquipmentSlot slot)
    {
        if (slot.isHaveItem)
        {
            slot.ItemImage.sprite = slot.itemSO.Icon;
            slot.ItemImage.enabled = true;
        }
        else
        {
            slot.ItemImage.sprite = null;
            slot.ItemImage.enabled = false;
        }
    }
}
