using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;


public class Interactuable : MonoBehaviour
{
    [Space]
    [Header("Interactuable Attributes")]
    public CursorController cursor;
    public InteractableText interactableText;
    private GameObject interactableObject;
    public DialogueSystemTrigger dialog;
    public DialogueSystemTrigger dialogTooFar;
    public AdventurePlayerMovement player;
    public Transform playerDestination;

    [Space]
    [Header("Mobile")]
    public float delayToShowText = 0.5f;
    private bool isTouched = false;
    public bool mobile;
    private bool actionsExecuted = false;


    private void Start()
    {
        if (interactableText != null) { interactableObject = interactableText.AsignText(); }
    }

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

        if (distance <= 0.2f && !actionsExecuted)  
        {
            player.interactuable = false;
            dialog.OnUse();
            actionsExecuted = true; 
        }
        else if (distance > 0.2f && actionsExecuted)
        {
            // Restablecer el estado si la distancia es mayor nuevamente
            actionsExecuted = false;
        }
    }

    private IEnumerator ShowTextWithDelay()
    {
        yield return new WaitForSeconds(delayToShowText);

        isTouched = true;
        cursor.setInteractuableCursor();
        interactableText.ShowText();
    }

    private void OnDisable()
    {
        if (interactableObject != null)
        {
            interactableObject.gameObject.SetActive(false);
        }
        
    }
}
