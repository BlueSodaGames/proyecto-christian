using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerY : MonoBehaviour
{
    public Transform target; // El personaje o la c�mara principal
    public float parallaxFactor; // Cuanto m�s bajo, m�s lento se mueve el fondo

    private Vector3 startPos;
    public float offsetY = 0;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float distanceY = (target.position.y - startPos.y) * parallaxFactor;
        transform.position = new Vector2(transform.position.x, startPos.y + distanceY + offsetY);

    }
}
