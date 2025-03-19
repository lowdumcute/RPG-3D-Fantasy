using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [Header("Item Slot Data")] // Dữ Liệu của Item
    [SerializeField] public EquipItemSO itemSO;
    [SerializeField] public int ID;
    [SerializeField] public string ItemName;
    [SerializeField] public ItemType itemType;

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
    [Header("Drag Item")]
    public GameObject draggedItem;  // Đối tượng tạm để kéo hình ảnh
    public Image draggedImage;
    public GameObject parent;
    public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;
    [Header("Infor Object")]
    public GameObject StatPanel;
    public Image DecriptionImage;
    public TextMeshProUGUI DecriptionNameText;
    public TextMeshProUGUI DecriptionText;
    public TextMeshProUGUI AttackText;
    public TextMeshProUGUI DefenseText;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }
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
        Debug.Log("Reset Slot s");
        itemSO = null;
        ID = 0;
        ItemName = "";
        IconItemSprite = null;
        ItemDecription = "";
        isHaveItem = false;
        isSelected = false;   
        ItemImage.sprite = null;
        ItemImage.enabled = false;
        SelectedPanel.SetActive(false);

        

        
        
        
        
    }
    // Cập nhật lại UsingItem()
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
            StatPanel.SetActive(true);

        }
        else
        {
            SelectedPanel.SetActive(false);
            StatPanel.SetActive(false);

        }    
        ItemImage.sprite = IconItemSprite;
    }

    
    void OnLeftClick()//Nếu nhấn chuột trái reset lại tất cả UI panel đã chọn sang cái đang chọn
    {
        inventoryManager.DeSelectedAllItemSlot();

        isSelected = true;
        SelectedPanel.SetActive(true);
        
        DecriptionImage.sprite = IconItemSprite;
        DecriptionNameText.text = ItemName;
        DecriptionText.text = ItemDecription;
        AttackText.text = "";
        DefenseText.text = "";
        Debug.Log("OnPoint");
        
        // Đảm bảo chỉ thêm sự kiện cho nút khi chọn item       
        RefreshInfo();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)// kiểm tra điều kiện nhấn chuột trái
        {
            Debug.Log("OnPoint Left");
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
            Debug.Log("OnPoint right");
            EquipGear();
        }

    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {

        parentAfterDrag = transform.parent;
        
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(parent.transform);
        transform.position = Input.mousePosition;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        EquipmentSlot droppedSlot = eventData.pointerEnter?.GetComponentInChildren<EquipmentSlot>();
        if (droppedSlot != null && droppedSlot != this) // Nếu thả vào slot hợp lệ
        {
            transform.SetParent(parentAfterDrag);
            canvasGroup.blocksRaycasts = true;
            ResetItemSlot();
            return;
        }

        // Nếu không thả vào slot hợp lệ, quay lại vị trí ban đầu
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
       
         transform.position = Input.mousePosition; // Di chuyển theo chuột
        
    }

   
}
