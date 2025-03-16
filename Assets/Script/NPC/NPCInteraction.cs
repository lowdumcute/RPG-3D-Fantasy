using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject talkButton; // Button UI để nói chuyện
    [SerializeField] private NPCTalk npcTalk;
    [SerializeField] private GameObject CameraNpc;
    [SerializeField] private GameObject cameraMain;
    public float interactionRadius = 3f; // Bán kính phát hiện người chơi
    private Transform player;
    private bool isPlayerNear = false;
    private bool isInteracting = false;

    void Start()
    {
        if (talkButton != null)
            talkButton.SetActive(false); // Ẩn button ban đầu
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null && !isInteracting)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            isPlayerNear = distance <= interactionRadius;
            
            if (talkButton != null)
                talkButton.SetActive(isPlayerNear);

            if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
            {
                InteractWithNPC();
            }
        }
    }

    public void InteractWithNPC()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isInteracting = true;
        talkButton.SetActive(false);
        CameraNpc.SetActive(true);
        cameraMain.SetActive(false);
        GamePlayManager.Instance.LockPlayer();
        npcTalk.StartTalk();
        CanvasTalk.Instance.CloseButton.GetComponent<Button>().onClick.AddListener(EndInteraction);
        Debug.Log("Đang nói chuyện với NPC...");
        // Thêm logic hội thoại ở đây
    }

    public void EndInteraction()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        isInteracting = false;
        CameraNpc.SetActive(false);
        cameraMain.SetActive(true);
        GamePlayManager.Instance.UnlockPlayer();
        if (isPlayerNear && talkButton != null)
            talkButton.SetActive(true);
    }
}
