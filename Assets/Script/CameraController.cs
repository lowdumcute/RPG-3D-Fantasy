using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform followTarget;
    [SerializeField] private float distance = 7.5f;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float minVerticalAngle = 0;
    [SerializeField] private float maxVerticalAngle = 90;

    private Vector2 rotation;
    void Update()
    {
        rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotationSpeed;
        rotation.x = Mathf.Clamp(rotation.x, minVerticalAngle, maxVerticalAngle);
        var targetRotation = Quaternion.Euler(rotation);
        transform.position = followTarget.position - targetRotation * new Vector3(0f, 0f, distance);
        transform.rotation = targetRotation;
    }
}
