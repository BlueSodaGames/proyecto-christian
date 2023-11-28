using TMPro;
using UnityEngine;


public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime;
    public float updateInterval = 1f;
    private float timer;
    [HideInInspector] public float fps;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        timer += Time.unscaledDeltaTime;
        if (timer >= updateInterval)
        {
            fps = 1.0f / deltaTime;
            fpsText.text = "FPS: " + Mathf.Round(fps);
            timer = 0f;
        }
    }
}