using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerBasicEnemy : MonoBehaviour
{
    public float movementDistance = 5f;
    public float speed = 2f;

    private bool toRight = true;
    private Vector3 startPoint;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        MoverEnemigo();
    }

    void MoverEnemigo()
    {
        if (toRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            spriteRenderer.flipX = false;

        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            spriteRenderer.flipX = true;
        }

        // Verificar si el enemigo ha alcanzado la distancia máxima y cambiar la dirección.
        if (Mathf.Abs(transform.position.x - startPoint.x) >= movementDistance)
        {
            toRight = !toRight;
        }
    }
}
