using UnityEngine;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    public LockOnSystem lockOnSystem;
    public Image lockOnIcon;
    public Camera mainCamera;

    void Update()
    {
        if (lockOnSystem.currentTarget != null)
        {
            lockOnIcon.gameObject.SetActive(true);
            Vector3 screenPos = mainCamera.WorldToScreenPoint(lockOnSystem.currentTarget.position);
            lockOnIcon.transform.position = screenPos;
        }
        else
        {
            lockOnIcon.gameObject.SetActive(false);
        }
    }
}
