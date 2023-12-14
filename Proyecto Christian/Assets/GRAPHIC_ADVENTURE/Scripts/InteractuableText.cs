using UnityEngine;
using TMPro;

public class InteractableText : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text textPrefab;
    private TMP_Text currentText; 
    [SerializeField] private string message;
    [SerializeField] private GameObject canvas;
    private bool updatePositionText;


    public GameObject AsignText()
    {
        if (this.currentText != null)
        {
            return this.currentText.gameObject;
        }
        else
        {
            return null;
        }
        
    }

    public void ShowText()
    {
        currentText = Instantiate(textPrefab, canvas.transform);
        currentText.text = message;
        currentText.gameObject.SetActive(true);
        updatePositionText = true;
        
    }

    public void HideText()
    {
        currentText.gameObject.SetActive(false);
        updatePositionText = false;
        Destroy(currentText.gameObject);
    }

    private void Update()
    {
        if (updatePositionText)
        {
            // Actualiza la posición del texto para que siga al cursor.
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = 10f;
            currentText.rectTransform.position = new Vector2(cursorPosition.x, cursorPosition.y + 55);
        }
    }

}