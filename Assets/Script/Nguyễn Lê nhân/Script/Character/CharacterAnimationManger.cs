using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class CharacterAnimationManger : MonoBehaviour
{
    CharacterManager characterManager;
    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
    }
    public void UpdateParamaterValue(float Horizontal,float Vertical)
    {
        characterManager.animator.SetFloat("Horizontal", Horizontal,0.1f,Time.deltaTime);
        characterManager.animator.SetFloat("Vertical", Vertical, 0.1f, Time.deltaTime);
    }
    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformAction, bool applyRootMotion= true)
    {
        characterManager.animator.applyRootMotion = applyRootMotion;
        characterManager.animator.CrossFade(targetAnimation, 0.2f);

        characterManager.isPerformingAction = isPerformAction;
        
    }    
        

}
