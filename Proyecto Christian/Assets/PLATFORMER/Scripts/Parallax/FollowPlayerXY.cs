using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerXY : MonoBehaviour
{
    public Transform target; // El personaje o la cámara principal

    void Update()
    {
        transform.position = new Vector2(target.transform.position.x, target.transform.position.y);
    }
}
