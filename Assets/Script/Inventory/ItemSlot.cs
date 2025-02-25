using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerClickHandler
{
    [Header("Item Slot Data")] // Dữ Liệu của Item
    [SerializeField] public ItemSO itemSO;
    [SerializeField] public int ID;
    [SerializeField] private string ItemName;
    [SerializeField] public int ItemQuantity;
    [SerializeField] private Sprite IconItemSprite;
    [SerializeField] private string ItemDecription;
    public bool isHaveItem;

    [Header("Item Slot")]// Dữ liệu của Item nhưng ở ngoài UI
    public TextMeshProUGUI QuantityText;
    public Image ItemImage;
    public GameObject SelectedPanel;
    public bool isSelected;

    [Header("Item Decription")]// Phần UI hiện thông tin của vật phẩm
    public Image ItemDecriptionImage;
    public TextMeshProUGUI ItemDecriptionNameText;
    public TextMeshProUGUI ItemDecriptionText;
    public Button UsingItemButton;
    [Header("Invetory Manager")]    //Inventory của người chơi
    [SerializeField] private InventoryManager inventoryManager;
    public PlayerHealthManager PlayerHealth;// máu của người chơi
    
    private void Start()
    {
        RefreshInfo();
        inventoryManager = GameObject.Find("Inventory").gameObject.GetComponent<InventoryManager>();
        PlayerHealth = GameObject.Find("Player").gameObject.GetComponent<PlayerHealthManager>();


    }

    //Hàm gọi khi mới Add 1 vật phẩm mới vào Inventory
    public void AddItem(ItemSO itemSO,int quantity)
    {
        this.itemSO = itemSO;
        this.ItemName = itemSO.ItemName;
        this.ID = itemSO.ID;
        this.ItemQuantity = quantity;
        this.IconItemSprite = itemSO.Icon;
        this.ItemDecription = itemSO.Decription;
        QuantityText.text = ItemQuantity.ToString();
        QuantityText.enabled = true;
        ItemImage.sprite = IconItemSprite;
        isHaveItem = true;
    }
    //Hàm sử dụng Item
    // Hàm reset thông tin của ItemSlot khi ItemQuantity <= 0
    public void ResetItemSlot()
    {
        itemSO = null;
        ID = 0;
        ItemName = "";
        ItemQuantity = 0;
        IconItemSprite = null;
        ItemDecription = "";
        isHaveItem = false; 
        isSelected = false;

        QuantityText.text = "";
        QuantityText.enabled = false;
        ItemImage.sprite = null;
        SelectedPanel.SetActive(false);

        ItemDecriptionNameText.text = "";
        ItemDecriptionText.text = "";
        ItemDecriptionImage.sprite = null;
    }

    // Cập nhật lại UsingItem()
    public void UsingItem()
    {
        Debug.Log($"using {itemSO.ItemName}");
        if (itemSO.Status == StatusChange.Healt)
        {
            if(PlayerHealth.playerStats.currentHealth >= PlayerHealth.playerStats.maxHealth)
            {
                return;
            }
            else
            {
                PlayerHealth.playerStats.currentHealth += itemSO.NumberOfChange;
                if (PlayerHealth.playerStats.currentHealth >= PlayerHealth.playerStats.maxHealth)
                {
                    PlayerHealth.playerStats.currentHealth = PlayerHealth.playerStats.maxHealth;
                }
                PlayerHealth.UpdateHealthUI();
                Debug.Log($"Current Health:{PlayerHealth.playerStats.currentHealth}");
                RefreshInfo();
            }
            
        }
        else if (itemSO.Status == StatusChange.Mana)
        {
            // Xử lý sử dụng item cho Mana
            RefreshInfo();
        }
        else if (itemSO.Status == StatusChange.Stamina)
        {
            // Xử lý sử dụng item cho Stamina
            RefreshInfo();
        }

        // Giảm số lượng vật phẩm sau khi sử dụng
        ItemQuantity--;

        // Kiểm tra nếu số lượng <= 0 thì reset slot
        if (ItemQuantity <= 0)
        {
            ResetItemSlot();
        }
        else
        {
            RefreshInfo();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)// kiểm tra điều kiện nhấn chuột trái
        {
            if(!isHaveItem)
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
        UsingItemButton.onClick.RemoveAllListeners(); // Xóa sự kiện cũ để tránh gọi nhiều lần
        UsingItemButton.onClick.AddListener(UsingItem);
        RefreshInfo();

}
    void OnRightClick()//Nhấn chuột phải vào slot để sử dụng Item
    {
        UsingItem();
        RefreshInfo();
    }
    public  void RefreshInfo()//sau khi nhấn thì làm mới lại thông tin vật phẩm 
    {
        if(isHaveItem)
        {
            QuantityText.enabled = true;
            QuantityText.text = ItemQuantity.ToString();
        }
        else
        {
            QuantityText.enabled = false;
        }
        if(isSelected)
        {
            ItemDecriptionImage.enabled = true;
            UsingItemButton.gameObject.SetActive(true);
        }
        else
        {
            ItemDecriptionImage.enabled = false;
            UsingItemButton.gameObject.SetActive(false);

        }
        ItemDecriptionNameText.text = ItemName;
        ItemDecriptionText.text = ItemDecription;
        ItemDecriptionImage.sprite = IconItemSprite;
    }

}
