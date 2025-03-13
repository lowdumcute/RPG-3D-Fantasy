using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageCollider : MonoBehaviour
{
    [Header("Damage")]
    public float PhysicalDamage = 0;
    public float MagicDamage = 0;
    public float LightingDamage = 0;
    public float FireDamage = 0;
    public float HolyDamage = 0;
    [Header("Character DAmaged")]
    public List<CharacterManager> CharacterDamaged = new List<CharacterManager>();
    
    [Header("contractPoint")]
    public Vector3 contractPoint;
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager player = other.gameObject.GetComponent<CharacterManager>();
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            contractPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            if(player != null)
            {
                DamageTarget(player);
                Debug.Log($"Dame ={PhysicalDamage}");
            }
        }
    }
    protected virtual void DamageTarget(CharacterManager DameTarget)
    {
        if (CharacterDamaged.Contains(DameTarget))
            return;
        CharacterDamaged.Add(DameTarget);
        TakingDamageEffect damageEfftect = Instantiate(WorldCharacterEffectmanager.instance.takeDamageEffectl);
        damageEfftect.PhysicalDamage = PhysicalDamage;
        damageEfftect.MagicDamage = MagicDamage;
        damageEfftect.FireDamage = FireDamage;
        damageEfftect.HolyDamage = HolyDamage;
        damageEfftect.contractPoint = contractPoint;


        DameTarget.characterEffectManager.ProcessInstantEffect(damageEfftect);
    }
}
