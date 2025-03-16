using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CanvasTalk : MonoBehaviour
{
    public static CanvasTalk Instance { get; private set; } // Singleton
    [SerializeField] private GameObject ButtonMissionPrefab; // Prefab của nút
    public TextMeshProUGUI dialogueText; // Text hiển thị hội thoại
    public GameObject Mission; 
    public GameObject choicePanel, CloseButton; // Panel chứa Yes/No
    [SerializeField] public List<GameObject> buttonsList = new List<GameObject>();  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Đảm bảo chỉ có 1 CanvasTalk
        }
    }
    public void Start()
    {
        CloseButton.SetActive(false);
        Mission.SetActive(false);
    }
     // Hàm tạo nút mới
    public void CreateButton(string buttonText, UnityEngine.Events.UnityAction onClickAction)
    {
        // Tạo một nút mới từ prefab ButtonMissionPrefab
        GameObject newButton = Instantiate(ButtonMissionPrefab, choicePanel.transform);  // Tạo nút từ prefab và đặt vào choicePanel
        newButton.name = buttonText;  // Đặt tên cho nút

        // Lấy TMP_Text từ con đầu tiên của nút (Giả sử TextMeshProUGUI là con đầu tiên của prefab)
        TMP_Text buttonTMPText = newButton.transform.GetChild(0).GetComponent<TMP_Text>();  // Lấy TMP từ con đầu tiên
        buttonTMPText.text = buttonText;  // Đặt nội dung cho nút

        // Cập nhật danh sách nút
        buttonsList.Add(newButton);

        // Thêm hành động khi nút được nhấn
        Button buttonComponent = newButton.GetComponent<Button>();  // Lấy component Button của nút mới
        buttonComponent.onClick.AddListener(onClickAction);  // Thêm hành động khi nhấn
    }

    // Hàm xóa nút
    public void RemoveButton(string buttonText)
    {
        // Duyệt qua danh sách các nút và tìm nút theo tên
        foreach (var button in buttonsList)
        {
            if (button.name == buttonText)
            {
                // Xóa nút khỏi danh sách và UI
                buttonsList.Remove(button);  // Xóa nút khỏi danh sách
                Destroy(button);  // Hủy nút khỏi UI
                break;  // Dừng vòng lặp sau khi tìm thấy và xóa nút
            }
        }
    }
}
