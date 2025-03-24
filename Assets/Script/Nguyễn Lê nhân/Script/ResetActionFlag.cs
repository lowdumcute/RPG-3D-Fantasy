using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager characterManager;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (characterManager == null)
        {
            characterManager = animator.GetComponent<CharacterManager>();
        }
        characterManager.isPerformingAction = false;
        characterManager.animator.applyRootMotion = false;
        
    }
    
}
