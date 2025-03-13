using System.Collections;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    [SerializeField] private Mission missionToGive;
    [TextArea(2, 5)] public string startDialogue;
    [TextArea(2, 5)] public string yesDialogue;
    [TextArea(2, 5)] public string noDialogue;
    public float typingSpeed = 0.05f;

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
            CanvasTalk.Instance.choicePanel.SetActive(true);
            CanvasTalk.Instance.yesButton.onClick.RemoveAllListeners();
            CanvasTalk.Instance.noButton.onClick.RemoveAllListeners();

            CanvasTalk.Instance.yesButton.onClick.AddListener(() => ChooseYes());
            CanvasTalk.Instance.noButton.onClick.AddListener(() => ChooseNo());
        }
    }

    public void ChooseYes()
    {
        CanvasTalk.Instance.choicePanel.SetActive(false);
        StartCoroutine(TypeSentence(yesDialogue, false));
        CanvasTalk.Instance.CloseButton.SetActive(true);
        MissionManager.Instance.AddMission(missionToGive);
    }

    public void ChooseNo()
    {
        CanvasTalk.Instance.choicePanel.SetActive(false);
        StartCoroutine(TypeSentence(noDialogue, false));
    }
}
