using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public Transform followTarget;

    [SerializeField] public float defaultDistance = 7.5f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float minVerticalAngle = 0;
    [SerializeField] private float maxVerticalAngle = 90;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float smoothingSpeed = 5f; // Giảm tốc độ Lerp xuống để tránh giật

    private Vector2 rotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float targetDistance; // Khoảng cách mong muốn
    [HideInInspector] public float currentDistance;
    public static bool isPaused = false;

    public LockOnSystem lockOnSystem; // Thêm biến tham chiếu Lock-On
    private bool isLockedOn => lockOnSystem != null && lockOnSystem.currentTarget != null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = defaultDistance;
        targetDistance = defaultDistance;
        Update();
    }

    void Update()
    {
        if (isPaused) return;

        if (isLockedOn && lockOnSystem.lockOnTargetPoint != null)
        {
            // Camera luôn nhìn vào mục tiêu khi khóa
            Vector3 directionToTarget = lockOnSystem.currentTarget.position - transform.position;

            // Giữ nguyên trục ngang (Y) nhưng hạn chế thay đổi trục dọc (X)
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            Vector3 eulerAngles = lookRotation.eulerAngles;

            eulerAngles.x = Mathf.LerpAngle(transform.eulerAngles.x, eulerAngles.x, Time.deltaTime * 2f); // Giảm rung
            eulerAngles.y = Mathf.LerpAngle(transform.eulerAngles.y, eulerAngles.y, Time.deltaTime * smoothingSpeed);

            transform.rotation = Quaternion.Euler(eulerAngles);
        }
        else
        {
            // Điều khiển camera bình thường nếu không khóa mục tiêu
            rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotationSpeed;
            rotation.x = Mathf.Clamp(rotation.x, minVerticalAngle, maxVerticalAngle);

            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;
        }

        Vector3 targetPosition = followTarget.position - transform.rotation * new Vector3(0f, 0f, currentDistance);
        
        RaycastHit hit;

        if (Physics.Raycast(followTarget.position, targetPosition - followTarget.position, out hit, defaultDistance, obstacleMask))
        {
            targetDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, defaultDistance);
        }

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothingSpeed);
       
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.1f);
        
    }
    public void SetTargetZoom(float zoomFactor)
    {
        targetDistance = Mathf.Lerp(minDistance, defaultDistance, zoomFactor);
    }
    public void SaveCurrentCameraRotation()
    {
        // Lưu góc quay hiện tại để không bị giật khi tắt Lock-on
        rotation.x = transform.eulerAngles.x;
        rotation.y = transform.eulerAngles.y;
    }


}


