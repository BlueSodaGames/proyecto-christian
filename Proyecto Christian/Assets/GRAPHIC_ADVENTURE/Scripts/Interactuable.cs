using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;


public class Interactuable : MonoBehaviour
{
    [Space]
    [Header("Interactuable Attributes")]
    public CursorController cursor;
    public InteractableText interactableText;
    public DialogueSystemTrigger dialog;
    public DialogueSystemTrigger dialogTooFar;
    public AdventurePlayerMovement player;
    public Transform playerDestination;

    [Space]
    [Header("Mobile")]
    public float delayToShowText = 0.5f; // El retraso en segundos antes de mostrar el texto.
    private bool isTouched = false;
    public bool mobile;
    private bool actionsExecuted = false;

    private void OnMouseEnter()
    {
        if (!Application.isMobilePlatform)
        {
            cursor.setInteractuableCursor();
            interactableText.ShowText();
        }
    }

    private void OnMouseExit()
    {
        if (!Application.isMobilePlatform)
        {
            cursor.setNormalCursor();
            interactableText.HideText();
        }
    }

    private void OnMouseDown()
    {
        player.targetPosition = (this.playerDestination.position);
        player.interactuable = true;
        actionsExecuted = false;
    }


    //MÓVIL
    private void Update()
    {
        float distance = Vector2.Distance(this.playerDestination.position, player.transform.position);

        if (distance <= 0.2f && !actionsExecuted)  // Verifica la distancia y si las acciones no se han ejecutado
        {
            player.interactuable = false;
            dialog.OnUse();
            actionsExecuted = true;  // Marca las acciones como ejecutadas
        }
        else if (distance > 0.2f && actionsExecuted)
        {
            // Restablecer el estado si la distancia es mayor nuevamente
            actionsExecuted = false;
        }
        if (mobile)
        {
            
            if (Input.touchCount > 0)
            {
                dialog.enabled = true;

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    StartCoroutine(ShowTextWithDelay());
                }
                else if (touch.phase == TouchPhase.Ended)
                {
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
