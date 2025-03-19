using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    CharacterAnimationManger characterAnimationManger;
    private void Start()
    {
        characterAnimationManger = GetComponent<CharacterAnimationManger>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

            characterAnimationManger.PlayTargetActionAnimation("Slide_Forward", true, true);
        }
    }
}
