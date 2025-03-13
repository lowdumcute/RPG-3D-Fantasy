using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectmanager : MonoBehaviour
{
    public static WorldCharacterEffectmanager instance;
    public TakingDamageEffect takeDamageEffectl;
    [SerializeField] List<InstantCharacterEffect> instantEffect;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GeneratedEffectID();
    }
    private void GeneratedEffectID()
    {
        for(int i = 0; i<instantEffect.Count;i++)
        {
            instantEffect[i].EffectID = i;
        }
    }
    
}
