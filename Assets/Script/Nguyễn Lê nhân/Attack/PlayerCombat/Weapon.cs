using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    public float dame;
    public BoxCollider boxCollider;
    public ParticleSystem particle;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        DisableTriggerBox();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Tính dame
    }
   
    //Mở Box

    public void EnableTriggerBox()
    {
        Debug.Log("Mở Hit box vũ khí");
        particle.Play();
        boxCollider.enabled = true;
        //Invoke("DisableTriggerBox", 0.3f);
    }
    //tắt Box 
    public void DisableTriggerBox()
    {
        Debug.Log("Tắt Hit box vũ khí");
        particle.Stop();
        boxCollider.enabled = false;

    }

}
