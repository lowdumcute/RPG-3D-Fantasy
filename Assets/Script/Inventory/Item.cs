using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum ItemType { Consume,weapon,Head,Body,Glove,Boots,Collectible}
public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private int Quantity;
    [SerializeField] private float pickupRange = 2f; // Khoảng cách để nhặt
    [SerializeField] private GameObject pickupUI; // UI thông báo nhặt
    [SerializeField] private ItemType itemType;
    private Transform player;

    private void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (pickupUI != null)
        {
            pickupUI.SetActive(false); // Ẩn UI khi bắt đầu
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            if (pickupUI != null) pickupUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Thêm item vào Inventory");

                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.AddItem(itemSO, Quantity, itemType);
                    Destroy(gameObject); // Xóa item sau khi nhặt
                }
                else
                {
                    Debug.LogWarning("InventoryManager chưa được khởi tạo!");
                }
            }
        }
        else
        {
            if (pickupUI != null) pickupUI.SetActive(false);
        }
    }
}
