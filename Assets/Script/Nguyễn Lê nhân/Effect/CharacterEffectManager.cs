using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class CharacterEffectManager : MonoBehaviour
{
    // Instance Effect: Blood lost, Frost Bite
    // Time Effect : poisom
    // Static Effect : Adding/Remove Buffs From Relic 


    public PlayerManager playerManager;
    public AudioSource audioSource;
    protected void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
    }
    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        //Take Effect 

        //Process It
        effect.ProcessEffect(playerManager);
    }
}
