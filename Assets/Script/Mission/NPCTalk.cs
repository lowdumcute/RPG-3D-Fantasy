using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCTalk : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Text để hiển thị hội thoại
    public GameObject choicePanel; // Panel chứa lựa chọn Yes/No
    public Button yesButton, noButton; // Nút Yes và No

    [TextArea(2, 5)] public string startDialogue; // Đoạn hội thoại mở đầu
    [TextArea(2, 5)] public string yesDialogue; // Đoạn hội thoại khi chọn Yes
    [TextArea(2, 5)] public string noDialogue; // Đoạn hội thoại khi chọn No

    public float typingSpeed = 0.05f; // Tốc độ chạy chữ

    private void Start()
    {
        choicePanel.SetActive(false); // Ẩn panel lựa chọn ban đầu
        StartCoroutine(TypeSentence(startDialogue, true)); // Bắt đầu chạy chữ cho đoạn mở đầu
    }

    IEnumerator TypeSentence(string sentence, bool showChoices)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (showChoices)
        {
            yield return new WaitForSeconds(0.5f);
            choicePanel.SetActive(true); // Hiện panel lựa chọn sau khi hội thoại chạy xong
        }
    }

    public void ChooseYes()
    {
        choicePanel.SetActive(false); // Ẩn lựa chọn
        StartCoroutine(TypeSentence(yesDialogue, false)); // Hiển thị hội thoại Yes
    }

    public void ChooseNo()
    {
        choicePanel.SetActive(false); // Ẩn lựa chọn
        StartCoroutine(TypeSentence(noDialogue, false)); // Hiển thị hội thoại No
    }
}
