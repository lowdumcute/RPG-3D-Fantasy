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
    [SerializeField] private float smoothingSpeed = 5f;

    private Vector2 rotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float targetDistance;
    [HideInInspector] public float currentDistance;
    public static bool isPaused = false;

    public LockOnSystem lockOnSystem;
    private bool isLockedOn => lockOnSystem != null && lockOnSystem.currentTarget != null;

    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    private List<Renderer> currentObstructions = new List<Renderer>();

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
    }

    void LateUpdate()
    {
        if (isPaused) return;

        if (isLockedOn && lockOnSystem.lockOnTargetPoint != null)
        {
            Vector3 directionToTarget = lockOnSystem.currentTarget.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            Vector3 eulerAngles = lookRotation.eulerAngles;
            eulerAngles.x = Mathf.LerpAngle(transform.eulerAngles.x, eulerAngles.x, Time.deltaTime * 2f);
            eulerAngles.y = Mathf.LerpAngle(transform.eulerAngles.y, eulerAngles.y, Time.deltaTime * smoothingSpeed);
            transform.rotation = Quaternion.Euler(eulerAngles);
        }
        else
        {
            rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotationSpeed;
            rotation.x = Mathf.Clamp(rotation.x, minVerticalAngle, maxVerticalAngle);
            Quaternion targetRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothingSpeed);
        }

        Vector3 targetPosition = followTarget.position - transform.rotation * Vector3.forward * currentDistance;

        RaycastHit hit;
        if (Physics.Raycast(followTarget.position, targetPosition - followTarget.position, out hit, defaultDistance, obstacleMask))
        {
            targetDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, defaultDistance);
        }
        else
        {
            targetDistance = defaultDistance;
        }

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothingSpeed);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.05f);

        HandleObstructions();
    }

    private void HandleObstructions()
    {
        foreach (Renderer renderer in currentObstructions)
        {
            if (renderer != null)
                renderer.material = originalMaterials[renderer];
        }
        currentObstructions.Clear();
        originalMaterials.Clear();

        Vector3 directionToPlayer = followTarget.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToPlayer, directionToPlayer.magnitude, obstacleMask);

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!originalMaterials.ContainsKey(renderer))
                {
                    originalMaterials[renderer] = renderer.material;
                    Material transparentMaterial = new Material(renderer.material);
                    transparentMaterial.color = new Color(transparentMaterial.color.r, transparentMaterial.color.g, transparentMaterial.color.b, 0.3f);
                    renderer.material = transparentMaterial;
                }
                currentObstructions.Add(renderer);
            }
        }
    }

    public void SetTargetZoom(float zoomFactor)
    {
        targetDistance = Mathf.Lerp(minDistance, defaultDistance, zoomFactor);
    }

    public void SaveCurrentCameraRotation()
    {
        rotation.x = transform.eulerAngles.x;
        rotation.y = transform.eulerAngles.y;
    }
}
