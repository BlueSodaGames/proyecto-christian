using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform portal;
    public Transform character;
    public float distanceFromCharacter = 2f; // Distancia a la que quieres que esté la flecha del personaje

    void Update()
    {
        if (portal != null && character != null)
        {
            // Calcula la dirección hacia el portal
            Vector3 directionToPortal = portal.position - character.position;

            // Calcula la posición de la flecha alrededor del personaje
            Vector3 arrowPosition = character.position + directionToPortal.normalized * distanceFromCharacter;
            transform.position = arrowPosition;

            // Calcula el ángulo para que la punta de la flecha apunte hacia el portal
            float angle = Mathf.Atan2(directionToPortal.y, directionToPortal.x) * Mathf.Rad2Deg;

            // Apunta la flecha hacia el portal
            transform.rotation = Quaternion.Euler(0, 0, angle);

        }
    }
}