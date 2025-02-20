using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimNMAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Gây 10 damage
            }
        }
    }
}
