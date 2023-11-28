using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class Timeline1EventHold : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool finishedHold;
    float tiempoMinimo = 1.0f; // Tiempo m�nimo que debe mantenerse presionada la tecla o bot�n en segundos.
    float tiempoActual = 0.0f;
    private void OnEnable()
    {
        finishedHold = false; 
        timeline.Pause();
    }

    // Update is called once per frame
    void Update()
    {

        if (!finishedHold)
        {
            if (Input.GetMouseButton(0)) // Reemplaza "KeyCode.A" con la tecla o bot�n que desees.
            {
                tiempoActual += Time.deltaTime; // Incrementa el tiempo actual mientras se mantiene presionada la tecla.

                if (tiempoActual >= tiempoMinimo)
                {
                    timeline.Resume();
                    finishedHold = true;
                    
                    // Realiza la acci�n que desees cuando se mantenga presionada la tecla durante el tiempo m�nimo.
                }
            }
            else
            {
                tiempoActual = 0.0f; // Reinicia el tiempo actual si la tecla se suelta.
                finishedHold = false;
            }
        }
    }

}
