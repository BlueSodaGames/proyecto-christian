using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    Vector2 newPos;
    public void Move(float delta)
    {
        newPos = transform.localPosition;
        newPos.x += delta * parallaxFactor;

        transform.localPosition = newPos;
    }


}
