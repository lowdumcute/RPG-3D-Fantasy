using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasTalk : MonoBehaviour
{
    public static CanvasTalk Instance { get; private set; } // Singleton

    public TextMeshProUGUI dialogueText; // Text hiển thị hội thoại
    public GameObject Mission; 
    public GameObject choicePanel, CloseButton; // Panel chứa Yes/No
    public Button yesButton, noButton; // Nút Yes/No

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
}
