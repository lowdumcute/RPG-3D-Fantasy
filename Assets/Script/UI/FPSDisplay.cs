using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";
    }
}
