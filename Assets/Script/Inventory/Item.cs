using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemSO item;
    [SerializeField] ItemSO item2;
    [SerializeField] private int Quantity;
    [SerializeField] private InventoryManager inventoryManager;
    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Add them item");
            inventoryManager.AddItem(item2, Quantity);
        }
    }
    public void OnTriggerEnter(Collider other)//Hàm chạm vào rồi nhặt vật phẩm 
    {
        if(other.gameObject.CompareTag(""))
        {
            Debug.Log($"Nhặt vật phẩm {item.ItemName}");
           // inventoryManager.AddItem(item, Quantity);
        }
    }
    // Update is called once per frame

}
