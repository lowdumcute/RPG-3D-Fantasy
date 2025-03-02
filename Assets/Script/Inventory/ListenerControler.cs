using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerControler : MonoBehaviour
{
    public GameObject cam;


    // Update is called once per frame
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
