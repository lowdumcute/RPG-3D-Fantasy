using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public GameObject Target;
    public Vector3 OffSet;
    public string NameTag;
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag(NameTag);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = Target.transform.position + OffSet;

            // Lấy góc quay của Camera nhưng khóa X và Z, chỉ giữ lại Y
            Vector3 cameraRotation = Camera.main.transform.rotation.eulerAngles;
            cameraRotation.x = 90f; // Nhìn từ trên xuống
            cameraRotation.z = 0f;  // Khóa lật ngang

            // Áp dụng góc quay mới
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
