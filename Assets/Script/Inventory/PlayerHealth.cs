using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth;
    public float CurrertHealth;
    void Start()
    {
        CurrertHealth = MaxHealth;
    }

    
}
