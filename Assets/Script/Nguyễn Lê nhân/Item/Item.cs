using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Informarion")]
    public int ItemID;
    public string ItemName;
    public Sprite ItemIcon;
    [TextArea]
    public string ItemDecriptions;
    

}
