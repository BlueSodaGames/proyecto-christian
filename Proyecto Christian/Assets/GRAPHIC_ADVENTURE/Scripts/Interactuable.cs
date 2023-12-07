using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using TMPro;
using Unity.VisualScripting;

public class Interactuable : MonoBehaviour
{
    [Space]
    [Header("Interactuable Attributes")]
    public CursorController cursor;
    public InteractableText interactableText;

    [Space]
    [Header("Mobile")]
    public float delayToShowText = 0.5f; // El retraso en segundos antes de mostrar el texto.
    private bool isTouched = false;
    public DialogueSystemTrigger dialog;
    public bool mobile;

    private void OnMouseEnter()
    {
        if (!Application.isMobilePlatform)
        {
            Debug.Log("Mouse Enter");
            cursor.setInteractuableCursor();
            interactableText.ShowText();
        }
    }

    private void OnMouseExit()
    {
        if (!Application.isMobilePlatform)
        {
            Debug.Log("Mouse Exit");
            cursor.setNormalCursor();
            interactableText.HideText();
        }
    }

    //MÓVIL
    private void Update()
    {

        if (mobile)
        {
            // Detectar el toque en dispositivos móviles.
            if (Input.touchCount > 0)
            {
                dialog.enabled = true;

                Touch touch = Input.GetTouch(0); // Obtener el primer toque.

                if (touch.phase == TouchPhase.Began)
                {
                    // Iniciar el retraso cuando se toca el objeto interactuable.
                    StartCoroutine(ShowTextWithDelay());
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    // Detener el retraso y restablecer la interacción cuando se levanta el dedo.
                    StopCoroutine(ShowTextWithDelay());
                    isTouched = false;
                    cursor.setNormalCursor();
                    interactableText.HideText();
                }
            }
        }
    }

    private IEnumerator ShowTextWithDelay()
    {
        yield return new WaitForSeconds(delayToShowText);

        isTouched = true;
        cursor.setInteractuableCursor();
        interactableText.ShowText();
    }
}
