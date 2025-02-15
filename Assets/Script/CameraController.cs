using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform followTarget;
    [SerializeField] private float defaultDistance = 7.5f; // Khoảng cách ban đầu
    [SerializeField] private float minDistance = 1.5f; // Khoảng cách tối thiểu khi chạm vật cản
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float minVerticalAngle = 0;
    [SerializeField] private float maxVerticalAngle = 90;
    [SerializeField] private LayerMask obstacleMask; // Lớp layer của vật cản (tường, cây,...)
    [SerializeField] private float smoothingSpeed = 10f; // Tốc độ làm mượt

    private Vector2 rotation;
    private float currentDistance; // Khoảng cách hiện tại của camera

    void Start()
    {
        currentDistance = defaultDistance;
    }

    void Update()
    {
        // Nhận input chuột để xoay camera
        rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotationSpeed;
        rotation.x = Mathf.Clamp(rotation.x, minVerticalAngle, maxVerticalAngle);
        Quaternion targetRotation = Quaternion.Euler(rotation);

        // Kiểm tra vật cản giữa camera và followTarget
        Vector3 targetPosition = followTarget.position - targetRotation * new Vector3(0f, 0f, defaultDistance);
        RaycastHit hit;
        
        if (Physics.Raycast(followTarget.position, targetPosition - followTarget.position, out hit, defaultDistance, obstacleMask))
        {
            // Nếu có vật cản, thu hẹp khoảng cách lại
            currentDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, defaultDistance);
        }
        else
        {
            // Nếu không có vật cản, phục hồi khoảng cách ban đầu (làm mượt)
            currentDistance = Mathf.Lerp(currentDistance, defaultDistance, Time.deltaTime * smoothingSpeed);
        }

        // Cập nhật vị trí và góc quay của camera
        transform.position = followTarget.position - targetRotation * new Vector3(0f, 0f, currentDistance);
        transform.rotation = targetRotation;
    }
}
