using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangePlayerSize : MonoBehaviour
{
    public float targetSize = 1f; // El tama�o al que se cambiar� gradualmente.
    public float transitionDuration = 1f; // Duraci�n de la transici�n en segundos.
    public AdventurePlayerMovement player;

    private float initialSize;
    private float startTime;
    private bool isTransitioning = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            initialSize = player.transform.localScale.x;
            startTime = Time.time;

            // Comenzar la transici�n de tama�o.
            StartCoroutine(ScalePlayerSize());
        }
    }

    private IEnumerator ScalePlayerSize()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // Calcular la escala interpolada en funci�n del tiempo transcurrido.
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            float newSize = Mathf.Lerp(initialSize, targetSize, t);

            // Aplicar la nueva escala al jugador.
            player.transform.localScale = new Vector3(newSize, newSize, 0);

            // Actualizar el tiempo transcurrido.
            elapsedTime = Time.time - startTime;

            yield return null;
        }

        // Asegurarse de que la escala sea exactamente igual al tama�o objetivo.
        player.transform.localScale = new Vector3(targetSize, targetSize, 0);

        // Restablecer el estado de transici�n.
        isTransitioning = false;
    }

}
