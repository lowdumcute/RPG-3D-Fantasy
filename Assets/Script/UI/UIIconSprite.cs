using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIIconSprite", menuName = "ScriptableObject/UIIconSprite")]
public class UIIconSprite : ScriptableObject
{
    [Header("Icon Nhiệm Vụ")]
    public Sprite IconMissionWait;
    public Sprite IconMissionComplete;
}

