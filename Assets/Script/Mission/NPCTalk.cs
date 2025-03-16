using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class NPCTalk : MonoBehaviour
{
    [SerializeField] public Mission missionToGive;
    [SerializeField] private GameObject MissionIcon;
    [TextArea(2, 5)] public string startDialogue;
    [TextArea(2, 5)] public string yesDialogue;
    [TextArea(2, 5)] public string EndDialogue;
    public float typingSpeed = 0.05f;
    private void Start()
    {
        UpdateMissionIcon();
    }

    private void OnTriggerEnter(Collider other) // Khi người chơi vào vùng kích hoạt
    {
        if (other.CompareTag("Player"))
        {
            StartTalk();
        }
    }

    public void StartTalk()
    {
        // Tìm đối tượng có tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Tính toán hướng từ NPC đến Player
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // Giữ nguyên trục Y để tránh NPC bị nghiêng

            // Quay NPC về hướng Player
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Hiển thị hộp thoại
        CanvasTalk.Instance.CloseButton.SetActive(true);
        CanvasTalk.Instance.choicePanel.SetActive(false);
        CanvasTalk.Instance.Mission.SetActive(true);
        StartCoroutine(TypeSentence(startDialogue, true));
    }

    IEnumerator TypeSentence(string sentence, bool showChoices)
    {
        CanvasTalk.Instance.dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            CanvasTalk.Instance.dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (showChoices)
        {
            yield return new WaitForSeconds(0.5f);
            // Xóa hết các nút cũ trong choicePanel
            foreach (Transform child in CanvasTalk.Instance.choicePanel.transform)
            {
                Destroy(child.gameObject);  // Xóa các nút con trong choicePanel
            }
            CanvasTalk.Instance.choicePanel.SetActive(true);
            CanvasTalk.Instance.CreateButton("Nothing", ChoseEndDialogue);
            if (!missionToGive.isActive && !missionToGive.isCompleted)
            {
                CanvasTalk.Instance.CreateButton("Asked Mission", AskedMission);
            }
        }
    }

    public void AskedMission()
    {
        // Hiển thị hội thoại về nhiệm vụ đã được hỏi
        StartCoroutine(TypeSentence(missionToGive.AskedMissionDialogue, false));
        // Tạo nút "Accept Mission"
        CanvasTalk.Instance.CreateButton("Accept Mission", AcceptMission);
        CanvasTalk.Instance.RemoveButton("Asked Mission");
        
    }

    public void AcceptMission()
    {
        CanvasTalk.Instance.choicePanel.SetActive(false);
        StartCoroutine(TypeSentence(missionToGive.AcceptMissionDialogue, false));
        CanvasTalk.Instance.CloseButton.SetActive(true);
        MissionManager.Instance.AddMission(missionToGive);
        UpdateMissionIcon();
    }

    public void ChoseEndDialogue()
    {
        CanvasTalk.Instance.choicePanel.SetActive(false);
        StartCoroutine(TypeSentence(EndDialogue, false));
        CanvasTalk.Instance.CloseButton.SetActive(true);
    }

    public void UpdateMissionIcon()
    {
        // Kiểm tra trạng thái mission và thay đổi icon
        if (!missionToGive.isActive)
        {
            MissionIcon.GetComponent<Image>().sprite = GamePlayManager.Instance.uiIconSprite.IconMissionWait;
        }
        else if (missionToGive.isActive && !missionToGive.isCompleted)
        {
            MissionIcon.SetActive(false); // Ẩn icon nếu mission đang hoạt động
        }
        else if (missionToGive.isActive && missionToGive.isCompleted)
        {
            MissionIcon.GetComponent<Image>().sprite = GamePlayManager.Instance.uiIconSprite.IconMissionComplete;
        }
    }
}
