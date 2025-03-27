using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    public float dame;
    public List<AttackSO> combo;
    public BoxCollider boxCollider;
    [Header("OverRide lại vũ khí")]
    public AnimatorOverrideController Weaponanimator;
    [Header("Âm thanh")]
    public AudioClip SoundAttack;
    public AudioClip SoundCollider;
    [SerializeField] private AudioSource audioSource;
    [Header("hiệu ứng")]
    [SerializeField] private GameObject VFX;
    public ParticleSystem particle;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        DisableTriggerBox();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            

            
                // Kiểm tra xem đối tượng có CharacterController hoặc tag "Enemy"
                if (other.TryGetComponent<CharacterController>(out CharacterController enemyController) || other.CompareTag("Enemy"))
                {
                    // Kiểm tra xem có EnemyHealth không
                    if (other.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                    {
                        // Gây sát thương
                        enemyHealth.TakeDamage(dame, transform.position);
                        Debug.Log("Hit VFX");
                    audioSource.PlayOneShot(SoundCollider);
                    var hitVFX =  Instantiate(VFX, transform.position, Quaternion.identity,transform); // Bật VFX
                    hitVFX.SetActive(true);


                }
                }
            
        }
    }
   
    //Mở Box

    public void EnableTriggerBox()
    {
        audioSource.PlayOneShot(SoundAttack);
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
