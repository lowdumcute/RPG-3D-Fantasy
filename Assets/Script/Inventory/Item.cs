using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemSO item;
    [SerializeField] ItemSO item2;
    [SerializeField] private int Quantity;
    [SerializeField] private InventoryManager inventoryManager;
    void Start()
    {
        Debug.Log("Cho vao ruong");
        inventoryManager.AddItem(item2, Quantity);


    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Add them item");
            inventoryManager.AddItem(item, Quantity);
        }
    }
    // Update is called once per frame

}
