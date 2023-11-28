using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class Timeline1EventClick : MonoBehaviour
{
    public PlayableDirector timeline;
    private bool finishedClick;
    float tiempoMinimo = 1.0f; // Tiempo mínimo que debe mantenerse presionada la tecla o botón en segundos.
    float tiempoActual = 0.0f;
    private void OnEnable()
    {
        timeline.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finishedClick)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    timeline.Resume();
                    finishedClick = true;
                }
            }

            for (int i = 0; i < 3; i++) // 0: izquierdo, 1: derecho, 2: central
            {
                if (Input.GetMouseButton(i))
                {
                    timeline.Resume();
                    finishedClick = true;
                }
            }
        }
    }
    private void OnDisable()
    {
        finishedClick = false;
    }
}
