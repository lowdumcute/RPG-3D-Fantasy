using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectmanager : CharacterEffectManager
{
    [SerializeField] InstantCharacterEffect effectTest;
    [SerializeField] bool process = false;

    private void Update()
    {
        if(process)
        {
            process = false;
            //thay đổi thông số SO bằng cách sử dụng Class muốn và set là As 
            //TakeStaminaDameEffect effect = Instantiate(effectTest) as TakeStaminaDameEffect;
            //effect.DameAmount = 7;
            // sau khi thay đổi thông số thì không ảnh hưởng đến chỉ số gốc 
            ProcessInstantEffect(effectTest);
            // Kiểm tra nếu effect có kiểu TakeStaminaDameEffect thì phát âm thanh
            if (effectTest is TakeStaminaDameEffect staminaEffect && staminaEffect.SoundEffect != null)
            {
                audioSource.PlayOneShot(staminaEffect.SoundEffect);
            }
        }
    }
}
