using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform followTarget;
    [SerializeField] private float defaultDistance = 7.5f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float minVerticalAngle = 0;
    [SerializeField] private float maxVerticalAngle = 90;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float smoothingSpeed = 10f;

    private Vector2 rotation;
    private float currentDistance;
    public static bool isPaused = false; // Biến kiểm soát camera

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = defaultDistance;
    }

    void Update()
    {
        if (isPaused) return; // Dừng input nếu isPaused = true

        rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotationSpeed;
        rotation.x = Mathf.Clamp(rotation.x, minVerticalAngle, maxVerticalAngle);
        Quaternion targetRotation = Quaternion.Euler(rotation);

        Vector3 targetPosition = followTarget.position - targetRotation * new Vector3(0f, 0f, defaultDistance);
        RaycastHit hit;
        
        if (Physics.Raycast(followTarget.position, targetPosition - followTarget.position, out hit, defaultDistance, obstacleMask))
        {
            currentDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, defaultDistance);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, defaultDistance, Time.deltaTime * smoothingSpeed);
        }

        transform.position = followTarget.position - targetRotation * new Vector3(0f, 0f, currentDistance);
        transform.rotation = targetRotation;
    }
}

