using UnityEngine;
using TMPro;

public class InteractableText : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text textPrefab; // El prefab de TextMeshPro que mostrar� encima del cursor.
    private TMP_Text currentText; // La instancia actual del texto.
    [SerializeField] private string message;
    [SerializeField] private GameObject canvas;
    private bool updatePositionText;
    

    
    private void Start()
    {
        currentText = Instantiate(textPrefab, canvas.transform);
        currentText.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        currentText.text = message;
        currentText.gameObject.SetActive(true);
        updatePositionText = true;
        
    }

    public void HideText()
    {
        currentText.gameObject.SetActive(false);
        updatePositionText = false;
    }

    private void Update()
    {
        if (updatePositionText)
        {
            // Actualiza la posici�n del texto para que siga al cursor.
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = 10f; // Ajusta la distancia Z para que el texto est� en frente de la c�mara.
            currentText.rectTransform.position = new Vector2(cursorPosition.x, cursorPosition.y + 55);
        }
        
    }

}