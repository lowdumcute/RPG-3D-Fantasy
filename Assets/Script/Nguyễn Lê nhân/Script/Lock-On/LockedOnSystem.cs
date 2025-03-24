using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public Transform currentTarget; // Mục tiêu đang khóa
    public float lockOnRadius = 10f; // Phạm vi khóa mục tiêu
    public LayerMask enemyLayer; // Layer kẻ địch
    public Transform lockOnTargetPoint; // Điểm trung gian để camera nhìn vào
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Nhấn Tab để khóa mục tiêu
        {
            if( currentTarget != null)
            {
                UnlockTarget();
            }
            else
            {
                LockOnToTarget();
            }
            
        }
    }

    void LockOnToTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRadius, enemyLayer);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = enemy.transform;
                closestDistance = distance;
            }
        }

        if (closestTarget != null)
        {
            currentTarget = closestTarget;
            Debug.Log("Locked on: " + currentTarget.name);
        }

    }
    void LockOnToNextTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRadius, enemyLayer);
        if (enemies.Length == 0) return;

        int currentIndex = -1;
        if (currentTarget != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].transform == currentTarget)
                {
                    currentIndex = i;
                    break;
                }
            }
        }

        int nextIndex = (currentIndex + 1) % enemies.Length;
        currentTarget = enemies[nextIndex].transform;
    }
    void UnlockTarget()
    {
        if (currentTarget != null)
        {
            if (CameraController.Instance != null)
            {
                CameraController.Instance.SaveCurrentCameraRotation(); // Giữ lại góc hiện tại
            }
            currentTarget = null;
        }
    }


}

