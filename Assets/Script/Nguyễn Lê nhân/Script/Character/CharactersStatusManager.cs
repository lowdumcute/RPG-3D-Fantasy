using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersStatusManager : MonoBehaviour
{
    [Header("Trạng thái")]
    public int Level;
    //Trạng thái vật lý
    public int Vigor; // Máu
    public int Endurence; // Thể lực
    public int Strength; // sức mạnh
    public int Dexterity;
    //Trạng thái ma thuật
    public int Mana;
    public int Intel;
    public bool isDead;
    
}
