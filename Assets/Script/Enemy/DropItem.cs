using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public List<GameObject> Items; // Danh sách các prefab có thể spawn
    public Transform spawnPoint;   // Vị trí spawn tùy chỉnh
    public float launchForce = 5f; // Lực đẩy lên trời

    public void Spawn()
    {
        if (Items.Count == 0) return; // Không có item để spawn

        // Xác định số lượng item sẽ spawn (ngẫu nhiên từ 1 đến số item tối đa)
        int itemCount = Random.Range(1, Items.Count + 1);

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, Items.Count); // Chọn item ngẫu nhiên
            Vector3 spawnPosition = spawnPoint ? spawnPoint.position : transform.position; // Dùng vị trí spawnPoint nếu có

            GameObject spawnedItem = Instantiate(Items[randomIndex], spawnPosition, Quaternion.identity);

            // Kiểm tra nếu item có Rigidbody thì đẩy lên trời
            Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
            }
        }
    }
}
