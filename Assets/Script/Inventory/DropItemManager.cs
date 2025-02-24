using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    public static DropItemManager Instance { get; private set; }

    [SerializeField] private List<GameObject> itemPrefabs; // Danh sách prefab item
    [SerializeField] private float spawnForce = 5f; // Lực bắn lên trời
    [SerializeField] private float torqueForce = 5f; // Lực xoay khi rơi
    [SerializeField] private int minDrop = 1; // Số lượng item tối thiểu có thể rơi
    [SerializeField] private int maxDrop = 3; // Số lượng item tối đa có thể rơi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnItems(Vector3 spawnPosition)
    {
        if (itemPrefabs.Count == 0)
        {
            Debug.LogWarning("DropItemManager: Không có prefab item nào trong danh sách!");
            return;
        }

        int dropCount = Random.Range(minDrop, maxDrop + 1); // Số lượng item rơi ngẫu nhiên
        for (int i = 0; i < dropCount; i++)
        {
            GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Count)]; // Chọn item ngẫu nhiên
            GameObject item = Instantiate(randomItem, spawnPosition, Quaternion.identity);

            if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                // Bắn lên trời với hướng ngẫu nhiên
                Vector3 forceDirection = Vector3.up * spawnForce + Random.insideUnitSphere * 2f;
                rb.AddForce(forceDirection, ForceMode.Impulse);

                // Tạo lực xoay ngẫu nhiên
                rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
            }

            // Hủy item sau 10 giây để tránh rác bộ nhớ
            Destroy(item, 10f);
        }
    }
}
