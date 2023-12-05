using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlashColor : MonoBehaviour
{
    public float flashDuration = 0.1f; // Duración del flash inicial
    public float fadeSpeed = 0.5f; // Velocidad de desvanecimiento
    public Color flashColor = Color.white; // Color del flash, puedes modificarlo en el Inspector
    public UnityEvent onFlashComplete;
    private Image fadeImage;
    public bool disable = false;

    private void OnEnable()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(1, 1, 1, 0); // Establece la transparencia inicial a 0
        StartCoroutine(FlashAndFade());
    }

    IEnumerator FlashAndFade()
    {
        // Flash inicial
        float flashStartTime = Time.time;
        while (Time.time < flashStartTime + flashDuration)
        {
            float alphaFlash = Mathf.Lerp(0f, 1f, (Time.time - flashStartTime) / flashDuration);
            fadeImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alphaFlash);
            yield return null;
        }

        if (onFlashComplete != null)
        {
            onFlashComplete.Invoke();
            
        }
        
        // Se queda en el color con opacidad máxima
        fadeImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1);

    }
}
