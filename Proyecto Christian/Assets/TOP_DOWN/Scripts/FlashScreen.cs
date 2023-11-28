using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreen : MonoBehaviour
{
    public float flashDuration = 0.1f; // Duración del flash inicial
    public float fadeSpeed = 0.5f; // Velocidad de desvanecimiento

    private Image fadeImage;

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
            fadeImage.color = new Color(1, 1, 1, alphaFlash);
            yield return null;
        }

        // Desvanecimiento
        float fadeStartTime = Time.time;
        while (fadeImage.color.a > 0)
        {
            float alphaFade = Mathf.Lerp(1f, 0f, (Time.time - fadeStartTime) * fadeSpeed);
            fadeImage.color = new Color(1, 1, 1, alphaFade);
            yield return null;
        }

        // Asegúrate de desactivar o destruir la imagen una vez que el desvanecimiento esté completo
        gameObject.SetActive(false);
        fadeImage.color = new Color(1, 1, 1, 1);
        // Alternativamente, puedes destruir el objeto si ya no lo necesitas
        // Destroy(gameObject);
    }
}