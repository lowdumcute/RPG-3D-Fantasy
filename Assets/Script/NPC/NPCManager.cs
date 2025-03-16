using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance; // Singleton
    [SerializeField] private List<NPCTalk> npcTalkList = new List<NPCTalk>();  // Danh sách các NPC có NPCTalk

    private void Start()
    {
        // Tìm tất cả các đối tượng có script NPCTalk
        npcTalkList.AddRange(FindObjectsOfType<NPCTalk>());
        UpdateAllMissionIcons(); // Cập nhật icon cho tất cả các NPC
    }

    // Cập nhật MissionIcon cho tất cả các NPCTalk trong danh sách
    public void UpdateAllMissionIcons()
    {
        foreach (NPCTalk npcTalk in npcTalkList)
        {
            if (npcTalk != null && npcTalk.missionToGive != null)
            {
                npcTalk.UpdateMissionIcon();  // Cập nhật icon cho từng NPC
            }
        }
    }

    // Gọi phương thức này khi cần cập nhật các nhiệm vụ, ví dụ khi một nhiệm vụ được hoàn thành
    public void UpdateMissionForNPC(Mission completedMission)
    {
        foreach (NPCTalk npcTalk in npcTalkList)
        {
            if (npcTalk.missionToGive == completedMission)
            {
                npcTalk.UpdateMissionIcon(); // Cập nhật icon của NPC đó sau khi nhiệm vụ hoàn thành
            }
        }
    }

    // Thêm NPC vào danh sách
    public void AddNPC(NPCTalk npcTalk)
    {
        if (!npcTalkList.Contains(npcTalk))
        {
            npcTalkList.Add(npcTalk);
            npcTalk.UpdateMissionIcon(); // Cập nhật icon khi NPC được thêm vào
        }
    }

    // Xóa NPC khỏi danh sách
    public void RemoveNPC(NPCTalk npcTalk)
    {
        if (npcTalkList.Contains(npcTalk))
        {
            npcTalkList.Remove(npcTalk);
        }
    }
}
