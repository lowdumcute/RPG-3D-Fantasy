using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int EffectID;
    public int instanceEffect;

    public virtual void ProcessEffect(PlayerManager playerManager)
    {

    }
   
}
