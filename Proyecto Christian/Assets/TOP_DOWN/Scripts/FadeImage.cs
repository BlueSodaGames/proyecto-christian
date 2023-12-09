using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Duración del desvanecimiento
    public Color initialColor = Color.white; // Color inicial, puedes modificarlo en el Inspector
    public UnityEvent onFadeComplete;
    private Image fadeImage;

    private void OnEnable()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = initialColor; // Establece el color inicial
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float startTime = Time.time;
        while (fadeImage.color.a > 0)
        {
            float alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration);
            fadeImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        if (onFadeComplete != null)
        {
            onFadeComplete.Invoke();
        }
    }
}
