using UnityEngine;
using TMPro;

public class NPCNameTag : MonoBehaviour
{
    public TextMeshProUGUI nameText;   // Tham chiếu đến UI hiển thị tên
    public Transform target;           // Mục tiêu mà UI luôn hướng tới (thường là Camera)
    public Vector3 offset = new Vector3(0, 2, 0); // Điều chỉnh vị trí hiển thị trên đầu NPC

    private void Start()
    {
        if (Camera.main != null)
        {
            target = Camera.main.transform; // Camera chính là mục tiêu
        }
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = transform.parent.position + offset; // Luôn ở trên đầu NPC
            transform.LookAt(transform.position + target.forward);   // Quay mặt về phía camera
        }
    }

    // Hàm thiết lập tên nhân vật
    public void SetName(string npcName, int level)
    {
        nameText.text = npcName + " - Lv." + level;
    }
}
