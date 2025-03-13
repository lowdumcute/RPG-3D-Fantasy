using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class UIStat_Bar : MonoBehaviour
{
    protected RectTransform rectTransform;
    public Slider slider;
    [Header("Bar Option")]
    [SerializeField] protected bool ScaleBarLenghtWithStat = false;
    [SerializeField] protected float widthScaleMultiplier = 1f;
    
   protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetStat(float newValue)
    {
        slider.value = newValue;
    }
    public void SetMaxStat(float MaxValue)
    {
        slider.maxValue = MaxValue;
        slider.value = MaxValue;
        if(ScaleBarLenghtWithStat)
        {
            //Scale the transform 
            rectTransform.sizeDelta = new Vector2(MaxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
            PlayerUI_HUD_Manager.instance.RefreshUI();
        }
    }

}
